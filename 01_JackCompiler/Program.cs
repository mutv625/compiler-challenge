class Program
{
    static void Main(string[] args)
    {
        string sourceDir = args.Length > 0 ? args[0] : @"C:\Users\mutv6\Desktop\Creative Programming P\01_JackCompiler\Source";

        foreach (string file in Directory.GetFiles(sourceDir, "*.jack"))
        {
            Console.WriteLine($"Compiling {file}...");

            Parser parser = new Parser(file);
            Class ast = parser.Compile();

            AstXmlWriter astXmlWriter = new AstXmlWriter(ast, Path.Combine(sourceDir, $"{Path.GetFileNameWithoutExtension(file)}.xml"));
            astXmlWriter.Write();

            Compiler compiler = new Compiler(ast, Path.Combine(sourceDir, $"{Path.GetFileNameWithoutExtension(file)}.vm"));
            compiler.Compile();
        }

        VmTranslator vmTranslator = new VmTranslator(sourceDir);
        vmTranslator.TranslateAll();

        Assembler assembler = new Assembler(sourceDir);
        assembler.AssembleAll();
            
    }
}