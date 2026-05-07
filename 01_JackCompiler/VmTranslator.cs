class VmTranslator
{
    string _inputDirPath;

    public VmTranslator(string inputDirPath)
    {
        _inputDirPath = inputDirPath;
    }

    public void TranslateAll()
    {
        // * 1. 入力ファイル群を取得
        string[] vmFiles = Directory.GetFiles(_inputDirPath, "*.vm");


        // * 2. 各ファイルを _ClassName.asm として出力
        foreach (string vmFile in vmFiles)
        {
            var translator = new VmWriter(vmFile, $"{_inputDirPath}/_{Path.GetFileNameWithoutExtension(vmFile)}.asm");
            translator.TranslateAndWrite();
        }


        // * 3. 出力ファイルを結合して最終出力ファイル Main.asm を作成
        using (var finalWriter = new StreamWriter($"{_inputDirPath}/Main.asm"))
        {
            // 3.1 _sys.asm と ブートストラップを最初に結合
            string sysOutFilePath = $"{_inputDirPath}/_Sys.asm";

            if (File.Exists(sysOutFilePath))
            {
                var sysReader = new StreamReader(sysOutFilePath);
                finalWriter.Write(AsmStrService.Bootstrap(256, false));

                finalWriter.Write(sysReader.ReadToEnd());
            }
            else
            {
                Console.WriteLine("[!!] _Sys.asm not found. Bootstrap code will not be included.");
            }

            // 3.2 _Main.asm を結合
            string mainFilePath = $"{_inputDirPath}/_Main.asm";
            
            if (File.Exists(mainFilePath))
            {
                var mainReader = new StreamReader(mainFilePath);
                finalWriter.Write(mainReader.ReadToEnd());
            }
            else
            {
                Console.WriteLine("[!!] _Main.asm not found.");
            }

            // 3.3 _Main.asm以外の各ファイルを結合
            foreach (string vmFile in vmFiles)
            {
                string asmFilePath = $"{_inputDirPath}/{Path.GetFileNameWithoutExtension(vmFile)}.asm";

                // _Sys.asm ,_Main.asm, 真のMain.asm はスキップ
                if (asmFilePath == sysOutFilePath || asmFilePath == mainFilePath || asmFilePath == $"{_inputDirPath}/Main.asm")
                {
                    continue;
                }

                var asmReader = new StreamReader(asmFilePath);
                finalWriter.Write(asmReader.ReadToEnd());
            }
        }
    }
}


class VmWriter
{
    // string inputFilePath;
    // string outputFilePath;
    // string fileName;

    // StreamReader reader;
    // StreamWriter writer;

    VmParser parser;
    // 1ファイルに付き1つのCodeWriterを用意し、ID被りがないようにする
    CodeWriter codeWriter;

    /// <summary>
    /// 入力にVMファイルパス、出力にASMファイルパスを指定します。
    /// TranslateAndWrite() で翻訳と出力ファイルの上書きを行います。
    /// </summary>
    /// <param name="inputFilePath">VMファイルパス</param>
    /// <param name="outputFilePath">ASMファイルの出力先、出力ファイル名</param>
    public VmWriter(string inputFilePath, string outputFilePath)
    {
        // this.inputFilePath = inputFilePath;
        // string outputFilePath = inputFilePath.Replace(".vm", ".asm");
        string fileName = Path.GetFileNameWithoutExtension(inputFilePath);

        var reader = new StreamReader(inputFilePath);
        var writer = new StreamWriter(outputFilePath);

        parser = new VmParser(reader);
        codeWriter = new CodeWriter(writer, fileName);
    }

    public void TranslateAndWrite()
    {
        while (parser.HasMoreLine())
        {
            Command command = parser.Advance();

            string segment = command.arg1;
            int index = command.arg2;

            switch (command.commandType)
            {
                case CommandType.C_ARITHMETIC:
                    codeWriter.WriteArithmetic(command.arg1);
                    break;

                case CommandType.C_PUSH:
                    codeWriter.WritePushPop(CommandType.C_PUSH, segment, index);
                    break;

                case CommandType.C_POP:
                    codeWriter.WritePushPop(CommandType.C_POP, segment, index);
                    break;


                case CommandType.C_LABEL:
                    codeWriter.WriteLabel(command.arg1);
                    break;

                case CommandType.C_GOTO:
                    codeWriter.WriteGoto(command.arg1);
                    break;

                case CommandType.C_IF:
                    codeWriter.WriteIf(command.arg1);
                    break;


                case CommandType.C_CALL:
                    codeWriter.WriteCall(command.arg1, command.arg2);
                    break;

                case CommandType.C_FUNCTION:
                    codeWriter.WriteFunction(command.arg1, command.arg2);
                    break;

                case CommandType.C_RETURN:
                    codeWriter.WriteReturn();
                    break;


                default:
                    throw new ArgumentException($"Cannot write unknown command type ({command.commandType}, {command.arg1}, {command.arg2}).");
            }
        }

        codeWriter.CloseWriter();
    }
}

class VmParser
{
    readonly StreamReader reader;

    public VmParser(StreamReader vmCodeReader)
    {
        reader = vmCodeReader;
    }

    public bool HasMoreLine() => !reader.EndOfStream;

    public Command CurrentCommand { get; private set; }


    /// <summary>
    /// 次の命令が存在する行をコマンドとして返します。
    /// CurrentCommand で現在読んでいるコマンドを取得できます。
    /// </summary>
    public Command Advance()
    {
        if (!HasMoreLine())
        {
            return new Command
            {
                commandType = CommandType.C_RETURN,
                arg1 = "",
                arg2 = 0
            };
        }

        string currentLine = (reader.ReadLine() ?? "").Trim();

        if (currentLine == "" || currentLine.StartsWith("//"))
        {
            return Advance();
        }

        // ! 行途中からのコメントを削除
        if (currentLine.Contains("//"))
        {
            int commentStartIndex = currentLine.IndexOf("//");
            currentLine = currentLine.Substring(0, commentStartIndex).Trim();
        }

        // トークンに分割
        string[] fields = currentLine.Split(" ");

        // * 1ワード系を先に例外処理
        if (fields.Length == 1)
        {
            if (fields[0] == "return")
            {
                return new Command
                {
                    commandType = CommandType.C_RETURN,
                    arg1 = "",
                    arg2 = 0
                };
            }
            else
            {
                return new Command
                {
                    commandType = CommandType.C_ARITHMETIC,
                    arg1 = fields[0],
                    arg2 = 0
                };
            }
        }

        switch (fields[0])
        {
            case "push":
                return new Command
                {
                    commandType = CommandType.C_PUSH,
                    arg1 = fields[1],
                    arg2 = int.Parse(fields[2])
                };
            case "pop":
                return new Command
                {
                    commandType = CommandType.C_POP,
                    arg1 = fields[1],
                    arg2 = int.Parse(fields[2])
                };


            case "label":
                return new Command
                {
                    commandType = CommandType.C_LABEL,
                    arg1 = fields[1],
                    arg2 = 0
                };

            case "goto":
                return new Command
                {
                    commandType = CommandType.C_GOTO,
                    arg1 = fields[1],
                    arg2 = 0
                };

            case "if-goto":
                return new Command
                {
                    commandType = CommandType.C_IF,
                    arg1 = fields[1],
                    arg2 = 0
                };


            case "call":
                return new Command
                {
                    commandType = CommandType.C_CALL,
                    arg1 = fields[1],
                    arg2 = int.Parse(fields[2])
                };

            case "function":
                return new Command
                {
                    commandType = CommandType.C_FUNCTION,
                    arg1 = fields[1],
                    arg2 = int.Parse(fields[2])
                };

            // case "return":
            //     return new Command
            //     {
            //         commandType = CommandType.C_RETURN,
            //         arg1 = "",
            //         arg2 = 0
            //     };


            default:
                throw new ArgumentException($"Cannot parse unknown command type ({fields[0]} in [{currentLine}]).");
        }
    }
}

enum CommandType
{
    C_ARITHMETIC,
    C_PUSH,
    C_POP,

    C_LABEL,
    C_GOTO,
    C_IF,
    C_FUNCTION,
    C_RETURN,
    C_CALL
}

struct Command
{
    public CommandType commandType;
    public string arg1;
    public int arg2;
}


class CodeWriter
{
    // readonly int initStackPointer = 256;

    readonly StreamWriter writer;
    AsmStrService asmStrService;
    string fileName = "";

    public CodeWriter(StreamWriter writer, string fileName)
    {
        this.writer = writer;
        asmStrService = new AsmStrService(fileName);
        this.fileName = fileName;
    }

    public void WriteArithmetic(string opcode)
    {
        string op = "";

        // 1. asm の演算子を決定する
        switch (opcode)
        {
            case "add":
                op = "+"; break;
            case "sub":
                op = "-"; break;
            case "and":
                op = "&"; break;
            case "or":
                op = "|"; break;

            case "neg":
                op = "-"; break;
            case "not":
                op = "!"; break;

            case "eq":
                op = "JEQ"; break;
            case "gt":
                op = "JGT"; break;
            case "lt":
                op = "JLT"; break;

            default:
                throw new ArgumentException($"Unknown Arithmetic Opcode ({opcode}).");
        }

        switch (opcode)
        {
            case "add":
            case "sub":
            case "and":
            case "or":
                writer.Write(asmStrService.Arithm2(op, false));
                return;

            case "neg":
            case "not":
                writer.Write(asmStrService.Arithm1(op, false));
                return;

            case "eq":
            case "gt":
            case "lt":
                writer.Write(asmStrService.Compr(op, false));
                return;
        }
    }

    public void WritePushPop(CommandType commandType, string segment, int index)
    {
        switch (commandType)
        {
            case CommandType.C_PUSH:
                writer.Write(asmStrService.Push(segment, index, false));
                break;

            case CommandType.C_POP:
                writer.Write(asmStrService.Pop(segment, index, false));
                break;
        }
    }

    public void WriteLabel(string symbol)
    {
        writer.Write(asmStrService.Label(symbol));
    }

    public void WriteGoto(string symbol)
    {
        writer.Write(asmStrService.Goto(symbol));
    }

    public void WriteIf(string symbol)
    {
        writer.Write(asmStrService.IfGoto(symbol));
    }

    public void WriteCall(string func, int nArgs)
    {
        writer.Write(asmStrService.Call(func, nArgs, false));
    }

    public void WriteFunction(string func, int nVars)
    {
        writer.Write(asmStrService.Function(func, nVars, false));
    }

    public void WriteReturn()
    {
        writer.Write(asmStrService.Return(false));
    }

    public void CloseWriter()
    {
        writer.Close();
    }
}

class AsmStrService
{
    static int uniqueComprId = 0;
    static int uniqueCallId = 0;

    string fileName = "";

    public AsmStrService(string fileName)
    {
        this.fileName = fileName;
    }

    public static string Bootstrap(int initSP, bool debug)
    {
        return @$"
// # Bootstrap code
    @{initSP}
    D=A
    @SP
    M=D

//// # call Sys.init 0
    // push @RET_ADDR of this call
        @_RET_ADDR$Sys.init_1
        D=A

        @SP
        A=M

        M=D

        // M[SP]++
        @SP
        M=M+1
    
    // push LCL = M[@LCL]
        @LCL
        D=M

        @SP
        A=M
        M=D

        // M[SP]++
        @SP
        M=M+1

    // push ARG = M[@ARG]
        @ARG
        D=M

        @SP
        A=M
        M=D

        // M[SP]++
        @SP
        M=M+1

    // push THIS = M[@THIS]
        @THIS
        D=M

        @SP
        A=M
        M=D

        // M[SP]++
        @SP
        M=M+1

    // push THAT = M[@THAT]
        @THAT
        D=M

        @SP
        A=M
        M=D

        // M[SP]++
        @SP
        M=M+1

    // new ARG = SP - 5 - nArgs
        @SP
        D=M

        @5
        D=D-A

        @0
        D=D-A

        @ARG
        M=D

    // new LCL = SP
        @SP
        D=M

        @LCL
        M=D
    
    // goto Sys.init
        @_f$Sys.init
        0;JMP

    // function returns here = @RET_ADDR
    (_RET_ADDR$Sys.init_1)
  
        ";
    }

    public string Arithm2(string op, bool debug)
    {
        return @$"
//// # add, sub, and, or
    // pop y to M[R13]
        @SP
        M=M-1
        A=M

        D=M

        @R13
        M=D

    // pop x to D
        @SP
        M=M-1
        A=M

        D=M

    // D = D(x) {op} M[R13](y)
        @R13
        D=D{op}M

    // push D
        @SP
        A=M

        M=D

        // SP++
        @SP
        M=M+1
        ";
    }

    public string Arithm1(string op, bool debug)
    {
        return $@"
//// # neg, not
    // pop to D
        @SP
        M=M-1
        A=M

        D=M

    // D = {op} D
        D={op}D

    // push D
        @SP
        A=M

        M=D

        // SP++
        @SP
        M=M+1
        ";
    }

    public string Compr(string op, bool debug)
    {
        uniqueComprId++;
        return $@"
////  # eq, lt, gt
    // pop y to M[R13]
        @SP
        M=M-1
        A=M

        D=M

        @R13
        M=D

    // pop x to D
        @SP
        M=M-1
        A=M

        D=M

    // D = D(x) - M[R13](y)
        @R13
        D=D-M
    
    // jump to @PUSH_TRUE if ({op})
        @_PUSH_TRUE${fileName}.{uniqueComprId}
        D;{op}
    // jump to @PUSH_FALSE else
        @_PUSH_FALSE${fileName}.{uniqueComprId}
        0;JMP

    (_PUSH_TRUE${fileName}.{uniqueComprId})
    // push True == 0xFFFF by !@0
        @0
        A=!A
        D=A

        @SP
        A=M

        M=D

        // SP++
        @SP
        M=M+1

        @_COMPR_END${fileName}.{uniqueComprId}
        0;JMP

    (_PUSH_FALSE${fileName}.{uniqueComprId})
    // push False == 0x0000
        @0
        D=A

        @SP
        A=M

        M=D

        // SP++
        @SP
        M=M+1

        @_COMPR_END${fileName}.{uniqueComprId}
        0;JMP

        (_COMPR_END${fileName}.{uniqueComprId})
        ";
    }

    public string Push(string segment, int value, bool debug)
    {
        // 1. セグメント名を決定する
        string segstr = "";

        switch (segment)
        {
            case "local":
                segstr = "LCL"; break;
            case "argument":
                segstr = "ARG"; break;
            case "this":
                segstr = "THIS"; break;
            case "that":
                segstr = "THAT"; break;

            case "pointer":
                switch (value)
                {
                    case 0:
                        segstr = "THIS"; break;
                    case 1:
                        segstr = "THAT"; break;
                    default:
                        throw new ArgumentException("Pointer index must be 0 or 1.");
                }
                break;

            case "temp":
                if (value < 0 || value > 7)
                {
                    throw new ArgumentException("Temp index must be in range 0-7.");
                }
                segstr = (5 + value).ToString(); break;

            case "static":
                segstr = $"_SV${fileName}.{value}"; break;

            default:
                break;
        }

        // 2. 出力文字列を決定する
        switch (segment)
        {
            case "local":
            case "argument":
            case "this":
            case "that":
                return $@"
//// # push SEGMENT index
    // A = M[@SEGMENT] + @index
        @{segstr}
        A=M
        D=A
        @{value}
        A=D+A

    // D = M[A] (now D have value to push)
        D=M

    // M[M[SP]] = D (value saved)
        @SP
        A=M
        M=D

    // SP++
        @SP
        M=M+1
                ";

            case "temp":
            case "pointer":
            case "static":
                return $@"
//// # push SEGMENT index
    // A = 5 + index
        @{segstr}

    // D = M[A] (now D have value to push)
        D=M

    // M[M[SP]] = D (value saved)
        @SP
        A=M
        M=D

    // SP++
        @SP
        M=M+1
                ";

            case "constant":
                return $@"
//// # push constant value
    // D = @value
        @{value}
        D=A
    
    // M[M[SP]] = D (value saved)
        @SP
        A=M
        M=D

    // SP++
        @SP
        M=M+1
                ";

            default:
                throw new ArgumentException("Unknown segment.");
        }
    }

    public string Pop(string segment, int value, bool debug)
    {
        // 1. セグメント名を決定する
        string segstr = "";

        switch (segment)
        {
            case "local":
                segstr = "LCL"; break;
            case "argument":
                segstr = "ARG"; break;
            case "this":
                segstr = "THIS"; break;
            case "that":
                segstr = "THAT"; break;

            case "pointer":
                switch (value)
                {
                    case 0:
                        segstr = "THIS"; break;
                    case 1:
                        segstr = "THAT"; break;
                    default:
                        throw new ArgumentException("Pointer index must be 0 or 1.");
                }
                break;

            case "temp":
                if (value < 0 || value > 7)
                {
                    throw new ArgumentException("Temp index must be in range 0-7.");
                }
                segstr = (5 + value).ToString(); break;

            case "static":
                segstr = $"_SV${fileName}.{value}"; break;

            default:
                break;
        }

        // 2. 出力文字列を決定する
        switch (segment)
        {
            case "local":
            case "argument":
            case "this":
            case "that":
                return $@"
//// # pop SEGMENT index
    // M[SP]--
        @SP
        M=M-1
    
    // D = M[ M[A(=SP)] ] (now D have a popped value)
        A=M
        D=M
    
    // M[R13] = D (pass the popped value from D to R13)
        @R13
        M=D

    // M[R14] = M[@SEGMENT] + @index (record the address to save the popped value)
        @{segstr}
        A=M
        D=A
        @{value}
        D=D+A

        @R14
        M=D
    
    // D = M[R13] (popped value)
        @R13
        D=M

    // M[M[R14]] = D
        @R14
        A=M
        M=D
                ";

            case "temp":
            case "pointer":
            case "static":
                return $@"
//// # pop SEGMENT index
    // M[SP]--
        @SP
        M=M-1
    
    // D = M[ M[A(=SP)] ] (now D have a popped value)
        A=M
        D=M
    
    // M[R13] = D (pass the popped value from D to R13)
        @R13
        M=D

    // M[R14] = (the address to save the popped value)
        @{segstr}
        D=A

        @R14
        M=D
    
    // D = M[R13] (popped value)
        @R13
        D=M

    // M[M[R14]] = D
        @R14
        A=M
        M=D
                ";


            case "constant":
                throw new ArgumentException("Cannot pop to constant segment.");
            default:
                throw new ArgumentException("Unknown segment.");
        }
    }


    public string Label(string symbol)
    {
        return $@"
//// label {symbol}
    (_L${symbol})
        ";
    }

    public string Goto(string symbol)
    {
        return $@"
//// goto {symbol}
    @_L${symbol}
        0;JMP
        ";
    }

    public string IfGoto(string symbol)
    {
        return $@"
//// if-goto {symbol}
    // pop cond to D
        @SP
        M=M-1
        A=M

        D=M

    // jump to @_L${symbol} if D != 0
        @_L${symbol}
        D;JNE

        ";
    }

/// <summary>
/// 
/// </summary>
/// <param name="func">呼び出す関数名</param>
/// <param name="nArgs">関数に渡す引数の個数</param>
/// <param name="debug"></param>
/// <returns></returns>
    public string Call(string func, int nArgs, bool debug)
    {
        uniqueCallId++;

        return $@"
//// # call {func} {nArgs}
    // push @RET_ADDR of this call
        @_RET_ADDR${func}_{uniqueCallId}
        D=A

        @SP
        A=M

        M=D

        // M[SP]++
        @SP
        M=M+1
    
    // push LCL = M[@LCL]
        @LCL
        D=M

        @SP
        A=M
        M=D

        // M[SP]++
        @SP
        M=M+1

    // push ARG = M[@ARG]
        @ARG
        D=M

        @SP
        A=M
        M=D

        // M[SP]++
        @SP
        M=M+1

    // push THIS = M[@THIS]
        @THIS
        D=M

        @SP
        A=M
        M=D

        // M[SP]++
        @SP
        M=M+1

    // push THAT = M[@THAT]
        @THAT
        D=M

        @SP
        A=M
        M=D

        // M[SP]++
        @SP
        M=M+1

    // new ARG = SP - 5 - nArgs
        @SP
        D=M

        @5
        D=D-A

        @{nArgs}
        D=D-A

        @ARG
        M=D

    // new LCL = SP
        @SP
        D=M

        @LCL
        M=D
    
    // goto function {func}
        @_f${func}
        0;JMP

    // function returns here = @RET_ADDR
    (_RET_ADDR${func}_{uniqueCallId})
        ";
    }

/// <summary>
/// 
/// </summary>
/// <param name="func">関数名</param>
/// <param name="nVars">関数のローカル変数の個数 (初期化でこの数分のメモリが確保されます)</param>
/// <param name="debug"></param>
/// <returns></returns>
    public string Function(string func, int nVars, bool debug)
    {
        return $@"
//// # function {func} {nVars} <REPEAT VER>
    (_f${func})
    
    // push 0 * {nVars} times; D = (counter)
        @{nVars}
        D=A
    
    (_f_INIT_LOOP${fileName}.{func})
        // end loop if D <= 0
            @_f_INIT_END${fileName}.{func}
            D;JLE

        // M[SP] = 0
            @SP
            A=M

            M=0
        
        // SP++
            @SP
            M=M+1

        // D(counter)--
            D=D-1

        // back to begin
            @_f_INIT_LOOP${fileName}.{func}
            0;JMP

    (_f_INIT_END${fileName}.{func})
        ";
    }

    public string Return(bool debug)
    {
        return @"
//// # return

    // R13(frameTop) = LCL
        @LCL
        D=M

        @R13
        M=D
    
    // R14(returnAddr) = M[D(frameTop) - 5]
        // A = D - 5
        @5
        A=D-A
        
        D=M

        @R14
        M=D

    // pop to M[ARG]
        @SP
        M=M-1
        A=M
        // now D have popped value
        D=M

        @ARG
        A=M
        M=D
    // SP = ARG + 1
        @ARG
        A=M
        A=A+1
        D=A
        
        @SP
        M=D


    // THAT = M[R13--]
        @R13
        M=M-1
        A=M

        D=M
        
        @THAT
        M=D

    // THIS = M[R13--]
        @R13
        M=M-1
        A=M

        D=M
        
        @THIS
        M=D

    // ARG = M[R13--]
        @R13
        M=M-1
        A=M

        D=M
        
        @ARG
        M=D
    
    // LCL = M[R13--]
        @R13
        M=M-1
        A=M

        D=M
        
        @LCL
        M=D

    // goto R14(returnAddr)
        @R14
        A=M
        0;JMP
        ";
    }
}

