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
            foreach (var child in _tree.Root.ChildNodes[0].ChildNodes)
            {
                if (child.Term.Name.Equals("end"))
                {
                    _cSharpCode.Add("\t}");
                    _cSharpCode.Add("}");
                    continue;
                }
                foreach (var children in child.ChildNodes)
                {
                    if (children.Term.Name.Equals("var"))
                    {
                        _cSharpCode.Add("using System;");
                        //_cSharpCode.Add("namespace test");
                        //_cSharpCode.Add("{");
                        _cSharpCode.Add("public class Test");
                        _cSharpCode.Add("{");
                        _cSharpCode.Add("\t static void Main()");
                        _cSharpCode.Add("\t{");
                        continue;
                    }
                    
                    if (children.ChildNodes.Count <= 0)
                    {
                        continue;
                    }
                    if (children.Term.Name.Equals("varDecl"))
                    {
                        if (children.ChildNodes[0].Term.Name.Equals("int"))
                        {
                            _cSharpCode.Add(children.FindTokenAndGetText() + " " + children.ChildNodes[1].FindTokenAndGetText() + ";");
                            continue;
                        }
                        else if (children.ChildNodes[0].Term.Name.Equals("list"))
                        {
                            _cSharpCode.Add("int[] " + children.ChildNodes[2].FindTokenAndGetText() +
                                " = new int[" + children.ChildNodes[1].FindTokenAndGetText() + "];");
                            continue;
                        }
                    }

                    NonControlFlow(children);

                    if (children.Term.Name.Equals("while"))
                    {
                        WhileMethod(children);
                        continue;
                    }
                    else if (children.Term.Name.Equals("if"))
                    {
                        FlowConditionalStatements(children);
                        _cSharpCode[_cSharpCode.Count - 1] += ")";
                        if (children.ChildNodes.Count == 6)
                        {
                            NonControlFlow(children.ChildNodes[5]);
                        }
                    }
                    else if (children.Term.Name.Equals("for"))
                    {
                        _cSharpCode.Add("for (" + children.ChildNodes[1].FindTokenAndGetText() + " = ");
                        var index = _cSharpCode.Count - 1;
                        var first = false;
                        ChildrenNodesRetrevial(children.ChildNodes[3], ref index, ref first);
                        _cSharpCode[index] += "; " + children.ChildNodes[1].FindTokenAndGetText() + " <= ";
                        ChildrenNodesRetrevial(children.ChildNodes[5], ref index, ref first);
                        _cSharpCode[index] += "; " + children.ChildNodes[1].FindTokenAndGetText() + "++){";
                        for (int i = 0; i < children.ChildNodes[6].ChildNodes.Count; i++)
                        {
                            if (children.ChildNodes[6].ChildNodes[i].Term.Name.Equals("if") ||
                                children.ChildNodes[6].ChildNodes[i].Term.Name.Equals("while"))
                            {
                                FlowConditionalStatements(children.ChildNodes[6].ChildNodes[i]);
                            }
                            else
                            {
                                NonControlFlow(children.ChildNodes[6].ChildNodes[i]);
                            }
                        }
                        _cSharpCode.Add("}");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="children"></param>
        private void WhileMethod(ParseTreeNode children)
        {
            FlowConditionalStatements(children);
            _cSharpCode[_cSharpCode.Count - 1] += "){";

            foreach (var item in children.ChildNodes[4].ChildNodes)
            {
                if (item.Term.Name.Equals("if"))
                {
                    FlowConditionalStatements(item);
                    _cSharpCode[_cSharpCode.Count - 1] += ")";
                    if (item.ChildNodes.Count == 6)
                    {
                        NonControlFlow(item.ChildNodes[5]);
                    }
                }
                else if (item.Term.Name.Equals("while"))
                {
                    FlowConditionalStatements(children);
                    _cSharpCode[_cSharpCode.Count - 1] += "){";
                    WhileMethod(item);
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
        /// <param name="children">Takes a node and checks for the above stated</param>
        private void NonControlFlow(ParseTreeNode children)
        {
            if (children.ChildNodes[0].Term.Name.Equals("label"))
            {
                _cSharpCode.Add(children.ChildNodes[1].FindTokenAndGetText() + ":");
            }
            else if (children.ChildNodes[0].Term.Name.Equals("input"))
            {
                _cSharpCode.Add(children.ChildNodes[1].FindTokenAndGetText() + " = Convert.ToInt32(Console.ReadLine());");
            }
            else if (children.ChildNodes[0].Term.Name.Equals("goto"))
            {
                _cSharpCode.Add(children.ChildNodes[0].FindTokenAndGetText() + " " + children.ChildNodes[1].FindTokenAndGetText() + ";");
            }
            else if (children.ChildNodes[0].Term.Name.Equals("prompt"))
            {
                _cSharpCode.Add("Console.Write(" + children.ChildNodes[1].FindTokenAndGetText() + ");");
            }
            else if (children.ChildNodes[0].Term.Name.Equals("let"))
            {
                _cSharpCode.Add(children.ChildNodes[1].FindTokenAndGetText());
                var index = _cSharpCode.Count - 1;
                var firstAdd = false;
                if (children.ChildNodes.Count <= 4)
                {
                    _cSharpCode[index] += " = ";
                    for (int i = 3; i < children.ChildNodes.Count; i++)
                    {
                        ChildrenNodesRetrevial(children.ChildNodes[i], ref index, ref firstAdd);
                    }

                }
                else
                {
                    _cSharpCode[index] += " [";
                    for (int i = 2; i < children.ChildNodes.Count; i++)
                    {
                        if (i == 3)
                        {
                            _cSharpCode[index] += "] ";
                        }
                        ChildrenNodesRetrevial(children.ChildNodes[i], ref index, ref firstAdd);
                    }
                }

                _cSharpCode[index] += ";";
            }
            else if (children.ChildNodes[0].Term.Name.Equals("print"))
            {
                _cSharpCode.Add("Console.Write(");
                var index = _cSharpCode.Count - 1;
                var first = false;
                ChildrenNodesRetrevial(children.ChildNodes[1], ref index, ref first);
                if (children.ChildNodes.Count > 2)
                {
                    _cSharpCode[index] += "[";
                    ChildrenNodesRetrevial(children.ChildNodes[2], ref index, ref first);
                    _cSharpCode[index] += "]";
                }
                
                _cSharpCode[index] += ");";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="children"></param>
        private void FlowConditionalStatements(ParseTreeNode children)
        {
            bool first = true;
            int index = 0;
            for (int i = 0; i < 4; i++)
            {
                if (first)
                {
                    _cSharpCode.Add(children.FindTokenAndGetText() + "(");
                    index = _cSharpCode.Count - 1;
                    first = false;
                }
                else
                {
                    foreach (var item in children.ChildNodes[i].ChildNodes)
                    {
                        ChildrenNodesRetrevial(item, ref index, ref first);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index"></param>
        /// <param name="firstAdd"></param>
        private void ChildrenNodesRetrevial(ParseTreeNode node, ref int index, ref bool firstAdd)
        {
            if (node.ChildNodes.Count > 0)
            {
                foreach (var item in node.ChildNodes)
                {
                    ChildrenNodesRetrevial(item, ref index, ref firstAdd);
                }
            }
            else
            {
                if (firstAdd)
                {
                    _cSharpCode.Add(node.FindTokenAndGetText() + " ");
                    index = _cSharpCode.Count - 1;
                    firstAdd = false;
                }
                else
                {
                    if (node.FindTokenAndGetText() != null)
                    {
                        _cSharpCode[index] = _cSharpCode[index] + node.FindTokenAndGetText() + " ";
                    }
                }
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