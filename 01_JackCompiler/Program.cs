// using System.Text;

// class Program
// {
//     static void Main(string[] args)
//     {
//         var tokenizer = new Tokenizer(@"C:\Users\mutv6\Desktop\Creative Programming P\01_JackCompiler\Prog.jack", Encoding.UTF8);
//         var tokenXmlWriter = new TokenXmlWriter(@"C:\Users\mutv6\Desktop\Creative Programming P\01_JackCompiler\token.xml");

//         while (tokenizer.HasMoreTokens)
//         {
//             tokenizer.Advance();

//             switch (tokenizer.CTokenType)
//             {
//                 case TokenType.KEYWORD:
//                     tokenXmlWriter.WriteLine(tokenizer.CTokenType, tokenizer.CKeyword.ToString().ToLower());
//                     break;
//                 case TokenType.SYMBOL:
//                     tokenXmlWriter.WriteLine(tokenizer.CTokenType, tokenizer.CSymbol.ToString());
//                     break;
//                 case TokenType.IDENTIFIER:
//                     tokenXmlWriter.WriteLine(tokenizer.CTokenType, tokenizer.CIdentifier);
//                     break;
//                 case TokenType.INTEGER_CONSTANT:
//                     tokenXmlWriter.WriteLine(tokenizer.CTokenType, tokenizer.CIntVal.ToString());
//                     break;
//                 case TokenType.STRING_CONSTANT:
//                     tokenXmlWriter.WriteLine(tokenizer.CTokenType, tokenizer.CStringVal);
//                     break;
//             }
//         }

//         tokenXmlWriter.Close();
//     }
// }

class Program
{
    static void Main(string[] args)
    {
        Parser parser = new Parser(@"C:\Users\mutv6\Desktop\Creative Programming P\01_JackCompiler\Source\Prog.jack", @"C:\Users\mutv6\Desktop\Creative Programming P\01_JackCompiler\Source");
        Class ast = parser.Compile();
        AstXmlWriter astXmlWriter = new AstXmlWriter(ast, @"C:\Users\mutv6\Desktop\Creative Programming P\01_JackCompiler\Source\Prog.xml");
        astXmlWriter.Write();
    }
}