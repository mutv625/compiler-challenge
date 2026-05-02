public class Program
{
    public static void Main()
    {
        string input = "- 12 + 34 * ( -5 * 2 + 5 ) / 3";
        var tokenizer = new ArithmTokenizer(input);

        List<Token> tokens = new();

        Console.WriteLine($"Input: {input}");
        while (tokenizer.GeneratedToken == null || tokenizer.GeneratedToken.Type != TokenType.TK_EOF)
        {
            tokenizer.ReadToken();
            if (tokenizer.GeneratedToken != null)
            {
                Console.WriteLine($"Generated Token: {tokenizer.GeneratedToken}");
                tokens.Add(tokenizer.GeneratedToken);
            }
        }

        ArithmParser parser = new ArithmParser(tokens);
        Expr ast = parser.ParseParent();
        Console.WriteLine($"Parsed AST:");
        AstPrinter.Print(ast);

    }
}