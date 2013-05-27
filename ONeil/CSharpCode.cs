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
                                _cSharpCode.Add(children.FindTokenAndGetText() + " " + children.ChildNodes[1].Token.Value.ToString());
                            }
                            else if (children.ChildNodes[0].Term.Name.Equals("list"))
                            {
                                _cSharpCode.Add(children.FindTokenAndGetText() + "[" + children.ChildNodes[1].Token.Value.ToString() + "] " +
                                    children.ChildNodes[2].Token.Value.ToString());
                            }
                        }

                        if (children.Term.Name.Equals("stmt"))
                        {

                        }
                        //_cSharpCode.Add(children.FindTokenAndGetText());
                    }
                }
            }
        }



        public override string ToString()
        {
            return base.ToString();
        }
    }
}
