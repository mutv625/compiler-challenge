class Program
{
    static void Main(string[] args)
    {
        string sourceDir = args.Length > 0 ? args[0] : @"C:\Users\mutv6\Desktop\Creative Programming P\01_JackCompiler\Source";

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

        foreach (string file in Directory.GetFiles(sourceDir))
        {
            string destFile = Path.Combine(outputDir, Path.GetFileName(file));
            File.Copy(file, destFile, true);
        }

        // 
        foreach (string file in Directory.GetFiles(outputDir, "*.jack"))
        {
            Parser parser = new Parser(file);
            Class ast = parser.Compile();

            AstXmlWriter astXmlWriter = new AstXmlWriter(ast, Path.Combine(outputDir, $"{Path.GetFileNameWithoutExtension(file)}.xml"));
            astXmlWriter.Write();

            Compiler compiler = new Compiler(ast, Path.Combine(outputDir, $"{Path.GetFileNameWithoutExtension(file)}.vm"));
            compiler.Compile();
        }

        VmTranslator vmTranslator = new VmTranslator(outputDir);
        vmTranslator.TranslateAll();

        Assembler assembler = new Assembler(outputDir);
        assembler.AssembleAll();
            
    }
}