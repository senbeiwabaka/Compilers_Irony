using System;
using Irony.Parsing;
using Irony.Interpreter.Ast;

namespace irony_test
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
            // adding them to the list
            base.NonGrammarTerminals.Add(remComment);

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
            var id = new NonTerminal("id");
            var idList = new NonTerminal("idList");
            var number = new NonTerminal("number");
            var digitList = new NonTerminal("digList");
            // title and begin are used for easy statement
            var title = new NonTerminal("title");
            var begin = new NonTerminal("begin");
            var end = new NonTerminal("end");
            var v = new NonTerminal("var");

            var whileStmt = new NonTerminal("while");
            var endWhile = new NonTerminal("endwhile");
            var forStmt = new NonTerminal("for");
            var endFor = new NonTerminal("endfor");
            // To correctly do if statements
            var ifStmt = new NonTerminal("if", typeof(IfNode));

            //3. non-terminals rule
            // Title line that accepts string literals with their built in newline
            title.Rule = ToTerm("title") + text + NewLine;
            begin.Rule = ToTerm("begin") + NewLine;
            end.Rule = ToTerm("end");
            programLine.Rule = title + v + varDecList + begin + stmtList + end;
            // VarDecList line that accepts their built in Empty element
            v.Rule = ToTerm("var") + NewLine;
            varDecList.Rule = MakeStarRule(varDecList, NewLine, varDecl);
            varDecTail.Rule = Empty | varDecl + varDecTail;
            varDecl.Rule = ToTerm("int") + id + NewLine | ToTerm("list") + "[" + digitList + "]" + id + NewLine;
            // MakeStarRule is zero or more elements <-- takes care of recursion
            stmtList.Rule = MakeStarRule(stmtList, NewLine, stmt);
            stmt.Rule = ToTerm("rem") + text |
                ToTerm("label") + identifier |
                "let" + id + "=" + expr |
                "let" + id + "[" + expr + "] =" + expr |
                ifStmt |
                "goto" + id |
                "input" + id |
                "input [" + id + "]" |
                "print" + expr |
                ToTerm("prompt") + text |
                whileStmt |
                forStmt |
                Empty;
            whileStmt.Rule = ToTerm("while") + "(" + expr + relOp + expr + ")" + NewLine + stmtList + "endwhile";// + NewLine;
            //endWhile.Rule = ToTerm("endwhile");
            ifStmt.Rule = ToTerm("if") + "(" + expr + relOp + expr + ")" + "then" + stmt |
                ToTerm("if") + "(" + expr + relOp + expr + ")" + "then" + NewLine;
            forStmt.Rule = ToTerm("for") + id + "=" + factor + "to" + factor + NewLine + stmtList + endFor |
                ToTerm("for") + id + "=" + factor + "to" + "(" + factor + ")" + NewLine + stmtList + endFor;// this is because of "size - 1"
            endFor.Rule = ToTerm("endfor");
            expr.Rule = term + exprTail;
            exprTail.Rule = Empty | addOp + term + exprTail;
            term.Rule = factor + termTail;
            termTail.Rule = Empty | mulOp + factor + termTail;
            factor.Rule = id | id + "[" + expr + "]" | number | "(" + expr + ")";
            relOp.Rule = ToTerm("<") | "==" | ">" | "<=" | ">=" | "!=";
            addOp.Rule = ToTerm("+") | "-";
            mulOp.Rule = ToTerm("*") | "/" | "%";
            // Use identifier to replace letter
            id.Rule = identifier + idList;
            idList.Rule = Empty | identifier + idList | digit + idList;
            number.Rule = digit + digitList | ToTerm("-") + digit + digitList;
            // Zero or more
            digitList.Rule = MakeStarRule(digitList, digit);
            program.Rule = MakePlusRule(program, null, programLine);
            // This signifys that this language uses Newline as line enders
            this.UsesNewLine = true;
            // I think this is for syntax checker and highlighter
            //this.MarkReservedWords("title", "rem", "var", "begin", "end", "endfor", "while", "endwhile", "prompt", "label");
            // This prunes the tree.
            //this.MarkPunctuation("[", "]", "(", ")",Environment.NewLine);
            //RegisterBracePair("(", ")");
            //RegisterBracePair("[", "]");
            //this.MarkTransient(begin, end, v, endWhile, title, endFor);
            this.Root = program;

            //this.LanguageFlags = LanguageFlags.CreateAst | LanguageFlags.NewLineBeforeEOF;
        }
    }
}