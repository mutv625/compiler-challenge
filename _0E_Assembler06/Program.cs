public class Program
{
    const string INPUT_FILE_PATH = "input";
    const string OUTPUT_FILE_PATH = "output";
    
    const int INITIAL_VAR_ADDR = 16;

    static Dictionary<string, int> builtinSymbol = new Dictionary<string, int>
    {
        {"R0", 0},
        {"R1", 1},
        {"R2", 2},
        {"R3", 3},
        {"R4", 4},
        {"R5", 5},
        {"R6", 6},
        {"R7", 7},
        {"R8", 8},
        {"R9", 9},
        {"R10", 10},
        {"R11", 11},
        {"R12", 12},
        {"R13", 13},
        {"R14", 14},
        {"R15", 15},
        {"SCREEN", 16384},
        {"KBD", 24576},
        {"SP", 0},
        {"LCL", 1},
        {"ARG", 2},
        {"THIS", 3},
        {"THAT", 4}
    };

    // ! C命令変換テーブル
    static Dictionary<string, ushort> cCompOpCodeDict = new Dictionary<string, ushort>
    {
        {"0", 0b0101010},
        {"1", 0b0111111},
        {"-1", 0b0111010},
        {"D", 0b0001100},
        {"A", 0b0110000},
        {"!D", 0b0001101},
        {"!A", 0b0110001},
        {"-D", 0b0001111},
        {"-A", 0b0110011},
        {"D+1", 0b0011111},
        {"A+1", 0b0110111},
        {"D-1", 0b0001110},
        {"A-1", 0b0110010},
        {"D+A", 0b0000010},
        {"D-A", 0b0010011},
        {"A-D", 0b0000111},
        {"D&A", 0b0000000},
        {"D|A", 0b0010101},
        {"M", 0b1110000},
        {"!M", 0b1110001},
        {"-M", 0b1110011},
        {"M+1", 0b1110111},
        {"M-1", 0b1110010},
        {"D+M", 0b1000010},
        {"D-M", 0b1010011},
        {"M-D", 0b1000111},
        {"D&M", 0b1000000},
        {"D|M", 0b1010101}
    };
    static Dictionary<string, ushort> cDestOpCodeDict = new Dictionary<string, ushort>
    {
        {"", 0b000},
        {"M", 0b001},
        {"D", 0b010},
        {"MD", 0b011},
        {"A", 0b100},
        {"AM", 0b101},
        {"AD", 0b110},
        {"AMD", 0b111}
    };
    static Dictionary<string, ushort> cJumpOpCodeDict = new Dictionary<string, ushort>
    {
        {"", 0b000},
        {"JGT", 0b001},
        {"JEQ", 0b010},
        {"JGE", 0b011},
        {"JLT", 0b100},
        {"JNE", 0b101},
        {"JLE", 0b110},
        {"JMP", 0b111}
    };

    public static void Main(string[] args)
    {
        StreamReader reader = new StreamReader(INPUT_FILE_PATH + ".asm");
        List<string> lines = new();

        List<ParsedLine> parsedLines = new();

        // * 1st Pass: すべての行を読む
        while (!reader.EndOfStream)
        {
            string? line = reader.ReadLine();
            if (!string.IsNullOrEmpty(line))
            {
                var parsedLine = ParseLine(line);
                if (parsedLine.Instruction != "")
                {
                    parsedLines.Add(parsedLine);
                }
            }
        }

        // * DEBUG
        Console.WriteLine("=== All Lines ===");
        foreach (var line in parsedLines)
        {
            Console.WriteLine(line);
        }

        // * 2nd Pass: カウンターとシンボルテーブルの構築
        // 定義済みシンボルを入れておく
        Dictionary<string, int> symbolTable = new(builtinSymbol);

        int instrAddr = 0;
        foreach (var line in parsedLines)
        {
            if (line.Instruction == "L")
            {
                string label = line.Operand[0];
                if (!symbolTable.ContainsKey(label))
                {
                    symbolTable[label] = instrAddr;
                    // ラベルのアドレスは「自分がそこにいなかったら下の命令に一致する」と考えると…
                }
            }
            else
            {
                instrAddr++;
            }
        }


        List<ushort> OpCodes = new();

        // * 3rd Pass: 機械語への変換
        // 変数用アドレスの初期値
        int nextVarAddr = INITIAL_VAR_ADDR;
        foreach (ParsedLine line in parsedLines)
        {
            if (line.Instruction == "A")
            {
                bool isNumber = int.TryParse(line.Operand[0], out int immedNum);
                if (isNumber)
                {
                    OpCodes.Add((ushort)immedNum);
                }
                else
                {
                    if (!symbolTable.ContainsKey(line.Operand[0]))
                    {
                        symbolTable[line.Operand[0]] = nextVarAddr;
                        nextVarAddr++;
                    }
                    OpCodes.Add((ushort)symbolTable[line.Operand[0]]);
                }
            }
            else if (line.Instruction == "C")
            {
                ushort cComp = cCompOpCodeDict[line.Operand[1]];
                ushort cDest = cDestOpCodeDict[line.Operand[0]];
                ushort cJump = cJumpOpCodeDict[line.Operand[2]];

                ushort opcode = (ushort)(((0b111) << 13) | (cComp << 6) | (cDest << 3) | cJump);
                OpCodes.Add(opcode);
            }
        }

        // * DEBUG 
        Console.WriteLine("=== Symbol Table ===");
        foreach (var sym in symbolTable)
        {
            Console.WriteLine(sym);
        }

        // * ファイル出力
        using StreamWriter writer = new StreamWriter(OUTPUT_FILE_PATH + ".hack");
        using BinaryWriter binWriter = new BinaryWriter(File.Open(OUTPUT_FILE_PATH + ".bin", FileMode.Truncate));

        for (int i = 0; i < OpCodes.Count; i++)
        {
            ushort opcode = OpCodes[i];
            {
                if (i == OpCodes.Count - 1)
                {
                    writer.Write(Convert.ToString(opcode, 2).PadLeft(16, '0'));
                }
                else
                {
                    writer.WriteLine(Convert.ToString(opcode, 2).PadLeft(16, '0'));
                }

                binWriter.Write(BitConverter.GetBytes(opcode)); // リトルエンディアンで書き込まれる
            }
        }

        writer.Close();
        binWriter.Close();
    }



    public struct ParsedLine
    {
        public string Instruction;
        public string[] Operand;

        public override string ToString()
        {
            return $"OP: {Instruction} [{string.Join(", ", Operand)}]";
        }
    }



    // 行 = 一つの文 をパースする
    public static ParsedLine ParseLine(string line)
    {
        line = line.Trim();
        if (line.StartsWith("//") || line == "")
        {
            return new ParsedLine { Instruction = "", Operand = new string[] { "" } };
        }
        else if (line.StartsWith("@"))
        {
            string symbol = line.Substring(1).Trim();
            return new ParsedLine { Instruction = "A", Operand = new string[] { symbol } };
        }
        else if (line.StartsWith("(") && line.EndsWith(")"))
        {
            string label = line.Substring(1, line.Length - 2).Trim();
            return new ParsedLine { Instruction = "L", Operand = new string[] { label } };
        }
        else
        {
            string dest = "";
            string comp = "";
            string jump = "";

            int eqIndex = line.IndexOf('=');
            int scIndex = line.IndexOf(';');

            if (eqIndex != -1)
            {
                dest = line.Substring(0, eqIndex).Trim();
                if (scIndex != -1)
                {
                    comp = line.Substring(eqIndex + 1, scIndex - eqIndex - 1).Trim();
                    jump = line.Substring(scIndex + 1).Trim();
                }
                else
                {
                    comp = line.Substring(eqIndex + 1).Trim();
                }
            }
            else
            {
                if (scIndex != -1)
                {
                    comp = line.Substring(0, scIndex).Trim();
                    jump = line.Substring(scIndex + 1).Trim();
                }
                else
                {
                    comp = line.Trim();
                }
            }

            return new ParsedLine { Instruction = "C", Operand = new string[] { dest, comp, jump } };
        }


    }
}