using VerySimpleInterpreter.Lexer;
using VerySimpleInterpreter;
using VerySimpleInterpreter.Parser;
using System;

namespace VerySimpleInterpreter
{

    internal class Program
    {

        public static void Main(string [] args)
        {

var st = new SymbolTable();
string Caminho = "path.txt";
var arq = new StreamReader(Caminho);
var basicLexer = new BasicLexer(arq, st);
var basicParser = new BasicParser(basicLexer, st);

basicParser.Prog();

// Token t = null;
// do {
//     t = basicLexer.GetNextToken();
//     Console.WriteLine($"<{t.Type},{t.Value}>");
// } while (t.Type != ETokenType.EOF);


//Console.WriteLine(st);
    }
}
}
