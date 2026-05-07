public class VmWriter
{
    StreamWriter writer;
    public VmWriter(string outDir, string outFile)
    {
        writer = new StreamWriter(Path.Combine(outDir, outFile));
    }

    public void WritePush(Segment segment, int index)
    {
        writer.WriteLine($"push {segment.ToString().ToLower()} {index}");
    }

    public void WritePop(Segment segment, int index)
    {
        writer.WriteLine($"pop {segment.ToString().ToLower()} {index}");
    }

    public enum Segment
    {
        CONSTANT,
        ARGUMENT,
        LOCAL,
        STATIC,
        THIS,
        THAT,
        POINTER,
        TEMP
    }

    public void WriteArithmetic(Command command)
    {
        writer.WriteLine(command.ToString().ToLower());
    }

    public enum Command
    {
        ADD,
        SUB,
        NEG,
        EQ,
        GT,
        LT,
        AND,
        OR,
        NOT
    }

    public void WriteLabel(string label)
    {
        writer.WriteLine($"label {label}");
    }

    public void WriteGoto(string label)
    {
        writer.WriteLine($"goto {label}");
    }

    public void WriteIf(string label)
    {
        writer.WriteLine($"if-goto {label}");
    }

    public void WriteCall(string name, int nArgs)
    {
        writer.WriteLine($"call {name} {nArgs}");
    }

    public void WriteFunction(string name, int nVars)
    {
        writer.WriteLine($"function {name} {nVars}");
    }

    public void WriteReturn()
    {
        writer.WriteLine("return");
    }

    public void Close()
    {
        writer.Close();
    }
}
