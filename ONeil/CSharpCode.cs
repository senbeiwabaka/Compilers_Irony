using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ONeil
{
    public class CSharpCode
    {
        private ParseTree _tree;
        private List<string> _cSharpCode;

        public string[] CSCode
        {
            get { return _cSharpCode.ToArray(); }
        }

        public CSharpCode(ParseTree tree)
        {
            _tree = tree;
            _cSharpCode = new List<string>(_tree.Tokens.Count);
        }

        public void GenerateCode()
        {
            foreach (var item in _tree.Root.ChildNodes)
            {
                foreach (var child in item.ChildNodes)
                {
                    foreach (var children in child.ChildNodes)
                    {
                        if (children.Term.Name.Equals("varDecl"))
                        {
                            if (children.ChildNodes[0].Term.Name.Equals("int"))
                            {
                                _cSharpCode.Add(children.FindTokenAndGetText() + " " + children.ChildNodes[1].FindTokenAndGetText() + ";");
                            }
                            else if (children.ChildNodes[0].Term.Name.Equals("list"))
                            {
                                _cSharpCode.Add(children.FindTokenAndGetText() + "[" + children.ChildNodes[1].FindTokenAndGetText() + "] " +
                                    children.ChildNodes[2].FindTokenAndGetText() + ";");
                            }
                        }

                        if (children.Term.Name.Equals("stmt") && children.ChildNodes.Count > 0)
                        {
                            if (children.ChildNodes[0].Term.Name.Equals("label"))
                            {
                                _cSharpCode.Add(children.ChildNodes[0].FindTokenAndGetText() + " " + children.ChildNodes[1].FindTokenAndGetText() + ":");
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
                            else
                            {
                                foreach (var elements in children.ChildNodes)
                                {
                                    //_cSharpCode.Add(elements.FindTokenAndGetText());
                                    var index = _cSharpCode.Count - 1;
                                    var firstAdd = true;
                                    ChildrenNodesRetrevial(elements, ref index, ref firstAdd);
                                }
                            }
                        }
                        //_cSharpCode.Add(children.FindTokenAndGetText());
                    }
                }
            }
        }

        private void ChildrenNodesRetrevial(ParseTreeNode node, ref int index, ref bool firstAdd)
        {
            if (node.ChildNodes.Count > 0)
            {
                if (node.ChildNodes[0].Term.Name.Equals("label"))
                {
                    _cSharpCode.Add(node.ChildNodes[0].FindTokenAndGetText() + " " + node.ChildNodes[1].FindTokenAndGetText() + ":");
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
                else
                {
                    foreach (var item in node.ChildNodes)
                    {
                        ChildrenNodesRetrevial(item, ref index, ref firstAdd);
                    }
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
                    _cSharpCode[index] = _cSharpCode[index] + node.FindTokenAndGetText() + " ";
                }
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
