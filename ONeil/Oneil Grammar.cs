using System;
using Irony.Parsing;

namespace ONeil
{
    [Language("O'Neil", "1.0", "O'Neil Language for Compilers")]
    public class OneilGrammar : Grammar
    {
        public OneilGrammar()
        {
            // The numbers or digit used. Set as int only but can be removed to allow all types
            var digit = new NumberLiteral("digit", NumberOptions.IntOnly | NumberOptions.AllowSign);
            // String(text) literal such as "Hello World!"
            var text = new StringLiteral("text", "\"", StringOptions.AllowsAllEscapes);
            // This is the variable
            var identifier = new IdentifierTerminal("identifier", IdOptions.IsNotKeyword);
            // Notifys the parser that "rem" is a comment
            var remComment = new CommentTerminal("comment", "rem", "\n", "\r");
            var title = new CommentTerminal("titleComment", "title", "\n", "\r");
            // adding them to the list
            base.NonGrammarTerminals.Add(remComment);
            base.NonGrammarTerminals.Add(title);

            //2. non-terminals
            var program = new NonTerminal("program");
            // Added so I could do something special to get program line to work
            var programLine = new NonTerminal("programLine");
            var varDecList = new NonTerminal("varDecList");
            var varDecTail = new NonTerminal("varDecTail");
            var varDecl = new NonTerminal("varDecl");
            var stmtList = new NonTerminal("stmtList");
            var stmt = new NonTerminal("stmt");
            var expr = new NonTerminal("expr");
            var exprTail = new NonTerminal("exprTail");
            var term = new NonTerminal("term");
            var termTail = new NonTerminal("termTail");
            var factor = new NonTerminal("factor");
            var relOp = new NonTerminal("relOp");
            var addOp = new NonTerminal("addOp");
            var mulOp = new NonTerminal("mulOp");
            //var id = new NonTerminal("id");
            //var idList = new NonTerminal("idList");
            //var number = new NonTerminal("number");
            //var digitList = new NonTerminal("digList");
            // title and begin are used for easy statement
            //var title = new NonTerminal("title");
            var begin = new NonTerminal("begin");
            var end = new NonTerminal("end");
            var v = new NonTerminal("var");


            var whileStmt = new NonTerminal("while");
            var endWhile = new NonTerminal("endwhile");
            var forStmt = new NonTerminal("for");
            var endFor = new NonTerminal("endfor");
            // To correctly do if statements
            var ifStmt = new NonTerminal("if");

            var label = new NonTerminal("label");
            var prompt = new NonTerminal("prompt");
            var print = new NonTerminal("print");
            var input = new NonTerminal("print");
            var gotos = new NonTerminal("goto");
            var let = new NonTerminal("let");


            //3. non-terminals rule
            // Title line that accepts string literals with their built in newline
            //title.Rule = ToTerm("title") + text + NewLine;
            begin.Rule = ToTerm("begin") + NewLine;
            end.Rule = ToTerm("end");

            programLine.Rule = NewLine + v + varDecList + begin + stmtList + end;
            // VarDecList line that accepts their built in Empty element
            v.Rule = ToTerm("var") + NewLine;
            varDecList.Rule = MakeStarRule(varDecList, varDecl);
            //varDecTail.Rule = Empty | varDecl + varDecTail;
            varDecl.Rule = ToTerm("int") + identifier + NewLine |
                ToTerm("list") + "[" + digit + "]" + identifier + NewLine |
                ToTerm("table") + "[" + digit + "," + digit + "]" + identifier + NewLine;

            label.Rule = ToTerm("label") + identifier;
            prompt.Rule = ToTerm("prompt") + text;
            print.Rule = ToTerm("print") + identifier |
                ToTerm("print") + identifier + "[" + expr + "]";
            input.Rule = ToTerm("input") + identifier |
                ToTerm("input") + "[" + identifier + "]";
            gotos.Rule = ToTerm("goto") + identifier;
            let.Rule = "let" + identifier + "=" + expr |
                "let" + identifier + "[" + expr + "]" + "=" + expr;

            // MakeStarRule is zero or more elements <-- takes care of recursion
            stmtList.Rule = MakeStarRule(stmtList, NewLine, stmt);
            stmt.Rule = label |
                let |
                ifStmt |
                gotos |
                input |
                print |
                prompt |
                whileStmt |
                forStmt |
                Empty;
            whileStmt.Rule = ToTerm("while") + "(" + expr + relOp + expr + ")" + NewLine + stmtList + endWhile;// + NewLine;
            endWhile.Rule = ToTerm("endwhile");
            ifStmt.Rule = ToTerm("if") + "(" + expr + relOp + expr + ")" + "then" + stmt;
            forStmt.Rule = ToTerm("for") + identifier + "=" + expr + "to" + expr + NewLine + stmtList + endFor;
            endFor.Rule = ToTerm("endfor");
            expr.Rule = term + exprTail;
            exprTail.Rule = Empty | addOp + term + exprTail;
            term.Rule = factor + termTail;
            termTail.Rule = Empty | mulOp + factor + termTail;
            factor.Rule = identifier | identifier + "[" + expr + "]" | digit | "(" + expr + ")";
            relOp.Rule = ToTerm("<") | "==" | ">" | "<=" | ">=" | "!=";
            addOp.Rule = ToTerm("+") | "-";
            mulOp.Rule = ToTerm("*") | "/" | "%";
            program.Rule = MakeStarRule(program, programLine);
            this.Root = program;

            // This signifies that this language uses Newline as line enders
            this.UsesNewLine = true;

            // I think this is for syntax checker and highlighter
            this.MarkReservedWords("var", "begin", "end", "endfor", "while", "endwhile");
            this.MarkPunctuation("[", "]", "(", ")", Environment.NewLine);
            this.MarkPunctuation(end, begin, endWhile, endFor);
            RegisterBracePair("(", ")");
            RegisterBracePair("[", "]");
            // This prunes the tree.
            this.MarkTransient(begin, end, endWhile, endFor, stmt);


            // this is flags for things like auto Ast creation but only works when every non transient node has an Ast
            //this.LanguageFlags = LanguageFlags.CreateAst | LanguageFlags.NewLineBeforeEOF;

            //this was my attempt at fixing the super recursive depth of the parsed tree
            //expr.Rule = identifier |
            //    digit |
            //    identifier + addOp + identifier |
            //    identifier + mulOp + identifier |
            //    digit + addOp + digit |
            //    digit + mulOp + digit |
            //    identifier + addOp + digit |
            //    identifier + mulOp + digit |
            //    digit + addOp + identifier |
            //    digit + mulOp + identifier |
            //    identifier + "[" + identifier + addOp + identifier + "]" |
            //    identifier + "[" + identifier + mulOp + identifier + "]" |
            //    identifier + "(" + identifier + addOp + identifier + ")" |
            //    identifier + "(" + identifier + mulOp + identifier + ")" |
            //    identifier + "[" + identifier + "]" |
            //    identifier + "(" + identifier + ")" |
            //    expr + "[" + expr + "]" + expr |
            //    expr + "(" + expr + ")" + expr |
            //    Empty;
        }
    }
}