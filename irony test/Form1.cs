using System;
using System.IO;
using System.Windows.Forms;
using Irony.Ast;
using Irony.Parsing;
using System.CodeDom.Compiler;
using ONeil;

namespace ONeil
{
    public partial class Form1 : Form
    {
        private Grammar _grammar;
        private LanguageData _language;
        private Parser _parser;
        private OneilGrammar _ONeilGrammar = new OneilGrammar();
        private ParseTree _parseTree;
        Form newForm;
        private CompilerResults results;

        public Form1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var open = new OpenFileDialog();
            open.InitialDirectory = Environment.CurrentDirectory + "\\Cases\\";
            open.Multiselect = false;
            open.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            open.FilterIndex = 1;

            var result = open.ShowDialog();

            if (result == DialogResult.OK)
            {
                var resultsReader = new StreamReader(open.FileName);

                txtONeilCode.Clear();

                txtONeilCode.Text = resultsReader.ReadToEnd();

                Text = "Output File : " + open.SafeFileName;

                resultsReader.Close();

                resultsReader = null;

                open = null;

                TreeCreation();
            }
        }

        /// <summary>
        /// creates the language and then parses the source file and generates/display the parsed tree
        /// </summary>
        private void TreeCreation()
        {
            _grammar = new Grammar(true);

            _grammar = _ONeilGrammar;

            _language = new LanguageData(_grammar);
            _language.AstDataVerified = true;
            _parser = new Parser(_language);

            _parser.Parse(txtONeilCode.Text);

            if (_parser.Root != null)
                _parseTree = _parser.Context.CurrentParseTree;

            // displays the errors
            if (_parseTree.HasErrors())
            {
                txtErrors.Clear();
                txtErrors.Text += "Parsed time: " + _parseTree.ParseTimeMilliseconds.ToString() + Environment.NewLine;
                foreach (var item in _parseTree.ParserMessages)
                {
                    txtErrors.Text += "Expected terminals: " + item.ParserState.ExpectedTerminals.ToString() + Environment.NewLine;
                    txtErrors.Text += "Message: " + item.Message + Environment.NewLine;
                    txtErrors.Text += "Location in source: " + item.Location.ToUiString() + Environment.NewLine;
                }
                txtErrors.Text += "The token value causing the error: " + _parseTree.Tokens[_parseTree.Tokens.Count - 1].Value.ToString() + Environment.NewLine;
                //txtErrors.Text += "The token type causing the error: " + _parseTree.Tokens[_parseTree.Tokens.Count - 1].EditorInfo.Type.ToString() + Environment.NewLine + Environment.NewLine;

                
            }
            else
            {
                txtErrors.Clear();
                txtErrors.Text += "Parsed time: " + _parseTree.ParseTimeMilliseconds.ToString() + Environment.NewLine;
            }

            ShowParseTree();
            
            ShowAstTree();
            
        }

        private void ShowParseTree()
        {
            tvParse.Nodes.Clear();
            if (_parser == null) return;
            AddParseNode(null, _parseTree.Root);
        }

        private void AddParseNode(TreeNode parent, ParseTreeNode node)
        {
            if (node == null) return;
            string txt = node.ToString();
            TreeNode tvNode = (parent == null ? tvParse.Nodes.Add(txt) : parent.Nodes.Add(txt));
            tvNode.Tag = node;
            foreach (var child in node.ChildNodes)
            {
                AddParseNode(tvNode, child);
            }
        }

        private void createTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeCreation();
        }

        private void ShowAstTree()
        {
            tvAstTree.Nodes.Clear();
            if (_parseTree == null || _parseTree.Root == null || _parseTree.Root.AstNode == null) return;
            AddAstNodeRec(null, _parseTree.Root.AstNode);
        }

        private void AddAstNodeRec(TreeNode parent, object astNode)
        {
            if (astNode == null) return;
            string txt = astNode.ToString();
            TreeNode newNode = (parent == null ?
              tvAstTree.Nodes.Add(txt) : parent.Nodes.Add(txt));
            newNode.Tag = astNode;
            var iBrowsable = astNode as IBrowsableAstNode;
            if (iBrowsable == null) return;
            var childList = iBrowsable.GetChildNodes();
            foreach (var child in childList)
                AddAstNodeRec(newNode, child);
        }

        private void addClassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newForm = new Form();
            newForm.Size = new System.Drawing.Size(400, 400);
            newForm.Show(this);
            var tb = new TextBox();
            tb.Multiline = true;
            tb.Size = new System.Drawing.Size(380, 300);
            tb.Location = new System.Drawing.Point(1, 1);
            tb.ScrollBars = ScrollBars.Vertical;
            newForm.Controls.Add(tb);
            var btn = new Button();
            btn.Size = new System.Drawing.Size(75, 23);
            btn.Location = new System.Drawing.Point(250, 325);
            btn.Text = "Submit Class";
            btn.Click += btn_Click;
            newForm.Controls.Add(btn);
            newForm.Resize += newForm_Resize;
        }

        void btn_Click(object sender, EventArgs e)
        {
            var codeProvider = CodeDomProvider.CreateProvider("CSharp");

            //generate exe not dll
            var par = new CompilerParameters
            {
                GenerateExecutable = false,
                GenerateInMemory = true,
                CompilerOptions = "/platform:x86 /optimize",
            };

            txtErrors.Clear();
            txtErrors.Text = newForm.Controls[0].Text;

            txtErrors.Clear();
            var errors = codeProvider.CompileAssemblyFromSource(par, newForm.Controls[0].Text);
            foreach (CompilerError item in errors.Errors)
            {
                txtErrors.Text += "line number " + item.Line + ", error num" + item.ErrorNumber +
                                     " , " + item.ErrorText + Environment.NewLine + Environment.NewLine;
            }

            if (errors.Errors.Count == 0)
            {
                var cde = errors.CompiledAssembly.GetType("myclass", false, true);
                if (cde != null)
                {
                    var mt = cde.GetMethod("Run");
                    var r = 2;
                    object[] param = new object[] { 1, r };
                    mt.Invoke(null, param);

                    txtErrors.Clear();
                    txtErrors.Text = param.GetValue(1).ToString();
                }
            }

        }

        void newForm_Resize(object sender, EventArgs e)
        {
            txtErrors.Text += newForm.Size.ToString() + Environment.NewLine;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void generatorCCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var csc = new CSharpCode(_parseTree);
            csc.GenerateCode();
            txtCSharpCode.Lines = csc.CSCode;
        }

        private void compileCCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var codeProvider = CodeDomProvider.CreateProvider("CSharp");

            txtErrors.Text = string.Empty;

            //generate exe not dll
            var par = new CompilerParameters
            {
                GenerateExecutable = true,
                OutputAssembly = "test.exe",
                CompilerOptions = "/platform:x86"
            };

            var _results = codeProvider.CompileAssemblyFromSource(par, txtCSharpCode.Text);


            if (_results.Errors.Count > 0)
            {
                foreach (CompilerError item in _results.Errors)
                {
                    txtErrors.Text += @"line number " + item.Line + @", error num" + item.ErrorNumber +
                                     @" , " + item.ErrorText + Environment.NewLine + Environment.NewLine;
                }
            }
            else
            {
                //Successful Compile
                txtErrors.Text = @"Success!";
            }
        }
    }
}