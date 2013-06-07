using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ONeil
{
    /// <summary>
    /// 
    /// </summary>
    public class CSharpCode
    {
        private ParseTree _tree;
        private List<string> _cSharpCode;

        /// <summary>
        /// Returns a string list of the code for a textbox
        /// </summary>
        public string[] CSCode
        {
            get { return _cSharpCode.ToArray(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tree"></param>
        public CSharpCode(ParseTree tree)
        {
            _tree = tree;
            _cSharpCode = new List<string>(_tree.Tokens.Count);
        }

        /// <summary>
        /// 
        /// </summary>
        public void GenerateCode()
        {
            //foreach (var child in _tree.Root.ChildNodes[0].ChildNodes)
            //{

            _cSharpCode.Add("using System;");
            //_cSharpCode.Add("namespace test");
            //_cSharpCode.Add("{");
            _cSharpCode.Add("public class Test");
            _cSharpCode.Add("{");
            _cSharpCode.Add("\t static void Main()");
            _cSharpCode.Add("\t{");

            foreach (var item in _tree.Root.ChildNodes[0].ChildNodes[1].ChildNodes)
            {
                if (item.ChildNodes[0].Term.Name.Equals("int"))
                {
                    _cSharpCode.Add(item.FindTokenAndGetText() + " " + item.ChildNodes[1].FindTokenAndGetText() + ";");
                    continue;
                }
                else if (item.ChildNodes[0].Term.Name.Equals("list"))
                {
                    _cSharpCode.Add("int[] " + item.ChildNodes[4].FindTokenAndGetText() +
                        " = new int[" + item.ChildNodes[2].FindTokenAndGetText() + "];");
                    continue;
                }
            }

            foreach (var children in _tree.Root.ChildNodes[0].ChildNodes[3].ChildNodes)
            {
                if (children.Term.Name.Equals("varDecl"))
                {

                }

                NonControlFlow(children);

                if (children.Term.Name.Equals("while"))
                {
                    WhileMethod(children);
                    continue;
                }
                else if (children.Term.Name.Equals("if"))
                {
                    IfMethod(children);
                }
                else if (children.Term.Name.Equals("for"))
                {
                    ForMethod(children);
                }
            }
            //}

            _cSharpCode.Add("\t}");
            _cSharpCode.Add("}");
        }

        private void ForMethod(ParseTreeNode children)
        {
            _cSharpCode.Add("for (" + children.ChildNodes[1].FindTokenAndGetText() + " = ");
            var index = _cSharpCode.Count - 1;
            ChildrenNodesRetrevial(children.ChildNodes[3], ref index);
            _cSharpCode[index] += "; " + children.ChildNodes[1].FindTokenAndGetText() + " <= ";
            ChildrenNodesRetrevial(children.ChildNodes[5], ref index);
            _cSharpCode[index] += "; " + children.ChildNodes[1].FindTokenAndGetText() + "++)";
            _cSharpCode.Add("{");
            for (int i = 0; i < children.ChildNodes[6].ChildNodes.Count; i++)
            {
                if (children.ChildNodes[6].ChildNodes[i].Term.Name.Equals("if"))
                {
                    FlowConditionalStatements(children.ChildNodes[6].ChildNodes[i]);
                }
                else if (children.ChildNodes[6].ChildNodes[i].Term.Name.Equals("while"))
                {
                    WhileMethod(children.ChildNodes[6].ChildNodes[i]);
                }
                else
                {
                    NonControlFlow(children.ChildNodes[6].ChildNodes[i]);
                }
            }
            _cSharpCode.Add("}");
        }

        private void IfMethod(ParseTreeNode node)
        {
            var index = 0;
            _cSharpCode.Add("if ( ");
            index = _cSharpCode.Count - 1;
            ChildrenNodesRetrevial(node.ChildNodes[2], ref index);
            ChildrenNodesRetrevial(node.ChildNodes[3], ref index);
            ChildrenNodesRetrevial(node.ChildNodes[4], ref index);
            _cSharpCode[index] += ")";
            //FlowConditionalStatements(node);
            //_cSharpCode[_cSharpCode.Count - 1] += ")";
            if (node.ChildNodes.Count == 8)
            {
                NonControlFlow(node.ChildNodes[7]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        private void WhileMethod(ParseTreeNode node)
        {
            var index = 0;
            _cSharpCode.Add("while (");
            index = _cSharpCode.Count - 1;
            ChildrenNodesRetrevial(node.ChildNodes[2], ref index);
            _cSharpCode[index] += node.ChildNodes[3].FindTokenAndGetText();
            ChildrenNodesRetrevial(node.ChildNodes[4], ref index);
            _cSharpCode[_cSharpCode.Count - 1] += ")";
            _cSharpCode.Add("{");
            foreach (var item in node.ChildNodes[6].ChildNodes)
            {
                if (item.Term.Name.Equals("if"))
                {
                    IfMethod(item);
                }
                else if (item.Term.Name.Equals("while"))
                {
                    WhileMethod(item);
                }
                else if (item.Term.Name.Equals("for"))
                {
                    ForMethod(item);
                }
                else
                {
                    NonControlFlow(item);
                }
            }
            _cSharpCode.Add("}");
        }

        /// <summary>
        /// Does labels, prints, prompts, gotos
        /// </summary>
        /// <param name="node">Takes a node and checks for the above stated</param>
        private void NonControlFlow(ParseTreeNode node)
        {
            if (node.ChildNodes[0].Term.Name.Equals("label"))
            {
                _cSharpCode.Add(node.ChildNodes[1].FindTokenAndGetText() + ":");
            }
            else if (node.ChildNodes[0].Term.Name.Equals("input"))
            {
                _cSharpCode.Add(node.ChildNodes[1].FindTokenAndGetText() + " = Convert.ToInt32(Console.ReadLine());");
            }
            else if (node.ChildNodes[0].Term.Name.Equals("goto"))
            {
                _cSharpCode.Add(node.ChildNodes[0].FindTokenAndGetText() + " " + node.ChildNodes[1].FindTokenAndGetText() + ";");
            }
            else if (node.ChildNodes[0].Term.Name.Equals("prompt"))
            {
                _cSharpCode.Add("Console.Write(" + node.ChildNodes[1].FindTokenAndGetText() + ");");
            }
            else if (node.ChildNodes[0].Term.Name.Equals("let"))
            {
                _cSharpCode.Add(node.ChildNodes[1].FindTokenAndGetText());
                var index = _cSharpCode.Count - 1;
                if (node.ChildNodes.Count <= 4)
                {
                    _cSharpCode[index] += " = ";
                    ChildrenNodesRetrevial(node.ChildNodes[3], ref index);
                }
                else
                {
                    _cSharpCode[index] += " [";
                    ChildrenNodesRetrevial(node.ChildNodes[3], ref index);
                    _cSharpCode[index] += "] " + node.ChildNodes[5].FindTokenAndGetText();
                    ChildrenNodesRetrevial(node.ChildNodes[6], ref index);
                }

                _cSharpCode[index] += ";";
            }
            else if (node.ChildNodes[0].Term.Name.Equals("print"))
            {
                _cSharpCode.Add("Console.Write(");
                var index = _cSharpCode.Count - 1;
                ChildrenNodesRetrevial(node.ChildNodes[1], ref index);
                if (node.ChildNodes.Count > 2)
                {
                    _cSharpCode[index] += "[";
                    ChildrenNodesRetrevial(node.ChildNodes[3], ref index);
                    _cSharpCode[index] += "]";
                }
                
                _cSharpCode[index] += ");";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        private void FlowConditionalStatements(ParseTreeNode node)
        {
            bool first = true;
            int index = 0;
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                if (first)
                {
                    _cSharpCode.Add(node.FindTokenAndGetText() + "(");
                    index = _cSharpCode.Count - 1;
                    first = false;
                }
                else
                {
                    foreach (var item in node.ChildNodes[i].ChildNodes)
                    {
                        ChildrenNodesRetrevial(item, ref index);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index"></param>
        /// 
        private void ChildrenNodesRetrevial(ParseTreeNode node, ref int index)
        {
            if (node.ChildNodes.Count > 0)
            {
                foreach (var item in node.ChildNodes)
                {
                    ChildrenNodesRetrevial(item, ref index);
                }
            }
            else if (node.FindTokenAndGetText() != null)
            {
                _cSharpCode[index] = _cSharpCode[index] + node.FindTokenAndGetText() + " ";
            }
        }

        /// <summary>
        /// Takes the whole built class and returns it as a single string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var tempString = new StringBuilder();
            foreach (var item in _cSharpCode)
            {
                tempString.Append(item);
            }

            return tempString.ToString();
        }
    }
}