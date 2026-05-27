using DotNetEnv;

class Program
{
    static void Main(string[] args)
    {
        var envInBase = Path.Combine(AppContext.BaseDirectory, ".env");
        if (File.Exists(envInBase)) Env.Load(envInBase);        

        string? sourceDir = args.Length > 0 ? 
            args[0] :
            Env.GetString("SOURCE_DIR");

        if (sourceDir == null) throw new ArgumentException("Source directory is not provided.");


        string outputDir = Path.Combine(sourceDir, "Output");
        if (Directory.Exists(outputDir))
        {
            Directory.Delete(outputDir, true);
            Directory.CreateDirectory(outputDir);
        }
        else
        {
            Directory.CreateDirectory(outputDir);
        }

        // # Library/*.jack -> Output/*.jack
        // ! Library/*.jack are not allowed to be modified.
        string libDir = Path.Combine(sourceDir, "Library");
        foreach (string file in Directory.GetFiles(libDir, "*.jack"))
        {
            string destFile = Path.Combine(outputDir, Path.GetFileName(file));
            File.Copy(file, destFile, true);
        }

        // # Source/Input/*.jack -> Output/*.jack
        string inputDir = Path.Combine(sourceDir, "Input");
        foreach (string file in Directory.GetFiles(inputDir, "*.jack"))
        {
            string destFile = Path.Combine(outputDir, Path.GetFileName(file));
            File.Copy(file, destFile, false);
        }

        // # Output/*.jack -> Output/*.xml,.vm
        foreach (string file in Directory.GetFiles(outputDir, "*.jack"))
        {
            Console.WriteLine($"Processing {file}...");

            // DEBUG Tokenizer
            Tokenizer tokenizer = new Tokenizer(file);
            TokenXmlWriter tokenXmlWriter = new TokenXmlWriter(Path.Combine(outputDir, $"{Path.GetFileNameWithoutExtension(file)}_tokens.xml"));
                while (tokenizer.HasMoreTokens)
                {
                    tokenizer.Advance();
                    switch (tokenizer.CTokenType)
                    {
                        case TokenType.KEYWORD:
                            tokenXmlWriter.WriteLine(tokenizer.CTokenType, tokenizer.CKeyword.ToString());
                            break;
                        case TokenType.SYMBOL:
                            tokenXmlWriter.WriteLine(tokenizer.CTokenType, tokenizer.CSymbol.ToString());
                            break;
                        case TokenType.IDENTIFIER:
                            tokenXmlWriter.WriteLine(tokenizer.CTokenType, tokenizer.CIdentifier);
                            break;
                        case TokenType.INTEGER_CONSTANT:
                            tokenXmlWriter.WriteLine(tokenizer.CTokenType, tokenizer.CIntVal.ToString());
                            break;
                        case TokenType.STRING_CONSTANT:
                            tokenXmlWriter.WriteLine(tokenizer.CTokenType, tokenizer.CStringVal);
                            break;
                    }
                }

            Parser parser = new Parser(file);
            Class ast = parser.Compile();

            AstXmlWriter astXmlWriter = new AstXmlWriter(ast, Path.Combine(outputDir, $"{Path.GetFileNameWithoutExtension(file)}.xml"));
            astXmlWriter.Write();

            Compiler compiler = new Compiler(ast, Path.Combine(outputDir, $"{Path.GetFileNameWithoutExtension(file)}.vm"));
            compiler.Compile();
        }

        // # Output/*.vm (files) -> Output/_Out.asm (single .asm file)
        VmTranslator vmTranslator = new VmTranslator(outputDir);
        vmTranslator.TranslateAll();

        // DEBUG VM Linker
        using (StreamWriter sw = new StreamWriter(Path.Combine(outputDir, "_Out.vm")))
        {
            foreach (string file in Directory.GetFiles(outputDir, "*.vm"))
            {
                if (Path.GetFileName(file) == "_Out.vm") continue;
                
                using (StreamReader sr = new StreamReader(file))
                {
                    sw.Write(sr.ReadToEnd());
                }
            }
        }
            

        // # Output/_Out.asm -> Output/_Out.hack
        Assembler assembler = new Assembler(outputDir);
        assembler.AssembleAll();
    }
}