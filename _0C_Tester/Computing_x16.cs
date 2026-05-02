using System;
using System.Text.RegularExpressions;

using BasicGates.Bit;
using static BasicGates.Bit.BooleanLogic;
using static Computing.Bit.BooleanCalc;

using BasicGates.x16;
using static BasicGates.x16.BooleanLogic16;
using static BasicGates.x16.SwitchingGate16;
using static Computing.x16.BooleanCalc16;


namespace Computing.x16;

public class BooleanCalc16
{
    public static (bool car, Bit16 s) Adder16(Bit16 a, Bit16 b, bool cin)
    {
        bool car = cin;
        Bit16 s = 0;
        for (int i = 0; i < 16; i++)
        {
            (car, s[i]) = FullAdder(a[i], b[i], car);
        }
        return (car, s);
    }

    public static Bit16 Negator16(Bit16 a)
    {
        Bit16 na = NOT16(a);
        (_, Bit16 negA) = Adder16(na, 1, false);

        return negA;
    }

    public static bool IsZero16(Bit16 a)
    {
        // bool isZr = true;
        // for (int i = 0; i < 16; i++)
        // {
        //     isZr = isZr & !a[i];
        // }
        return a == 0;
    }

    public static (Bit16 outp, bool isZr, bool isNg) ALU16(
        Bit16 x, Bit16 y,
        bool zx, bool nx,
        bool zy, bool ny,
        bool f, bool no)
    {
        // Preprocess x
        x = Mux2to1_16(zx, x, 0);
        x = Mux2to1_16(nx, x, NOT16(x));
        
        // Preprocess y
        y = Mux2to1_16(zy, y, 0);
        y = Mux2to1_16(ny, y, NOT16(y));

        // Function f
        Bit16 res = Mux2to1_16(f, AND16(x, y), Adder16(x, y, false).s);

        // Postprocess outp
        Bit16 outp = Mux2to1_16(no, res, NOT16(res));

        // isZr and isNg
        bool isZr = IsZero16(outp);
        bool isNg = outp[15];

        return (outp, isZr, isNg);
    }
}


public class PCounter16
{
    public Bit16 pcout;
    Register16 pcReg;

    public PCounter16()
    {
        pcout = Bit16Const.FALSE;
        pcReg = new Register16();
    }

    // ! clk == true のときに load する
    public Bit16 Process(Bit16 pcin, bool load, bool inc, bool reset, bool clk)
    {
        Bit16 incrRes = Mux2to1_16(inc, pcout, Adder16(pcout, 1, false).s);
        Bit16 loadRes = Mux2to1_16(load, incrRes, pcin);
        Bit16 din = Mux2to1_16(reset, loadRes, Bit16Const.FALSE);

        pcout = pcReg.process(din, true, clk);

        return pcout;
    }
}

public class CPUx16
{
    public PCounter16 pc;
    public Register16 rA;
    public Register16 rD;

    // * output
    public Bit16 outp;
    public bool writeM;
    public Bit16 addrM15;
    public Bit16 PCout15;

    public CPUx16()
    {
        pc = new PCounter16();
        rA = new Register16();
        rD = new Register16();
    }

    // 各パーツは既に完成されている。やるのみ。
    public (Bit16 outp, bool writeM, Bit16 addrM15, Bit16 PCout15) Process(Bit16 inM, Bit16 OP, bool rst, bool clk)
    {
        // TODO

        // set rD
        rD.process(outp, OP[15] & OP[4], clk);

        // set ALU.x
        Bit16 aluXin = rD.q;


        // set rA
        Bit16 dataA = Mux2to1_16(OP[15], OP, outp);
        bool loadA = !OP[15] || OP[5];

        rA.process(dataA, loadA, clk);

        // set ALU.y
        Bit16 aluYin = Mux2to1_16(OP[12], rA.q, inM);


        (outp, bool isZr, bool isNg) = ALU16(aluXin, aluYin, OP[11], OP[10], OP[9], OP[8], OP[7], OP[6]);

        // Process PCounter
        // ! jmpRes は、ALUの zr,ngを利用せよ！！！！ 
        bool jmpRes = XOR(OP[2],
            OR(AND(NOT(isNg), XOR(OP[2], OP[0])),
                AND(isZr, XOR(OP[2], OP[1]))
            )
        );
        
        pc.Process(rA.q, OP[15] & jmpRes, true, rst, clk);


        writeM = AND(OP[15], OP[3]);
        addrM15 = rA.q;
        PCout15 = pc.pcout;

        return (outp, writeM, addrM15, PCout15);
    }
}

public class ROM16
{
    public Bit16 output;
    public Bit16[] storage;

    public ROM16(Bit16[] program)
    {
        storage = program;
    }

    /// <summary>
    /// 01のバイナリの文字列を受け取り、自動的に16bitごとに分割して初期化します。
    /// </summary>
    /// <param name="program"></param>
    /// <param name="fromBase"></param>
    /// <exception cref="ArgumentException"></exception>
    public ROM16(string program, int fromBase)
    {
        // ref : https://learn.microsoft.com/ja-jp/dotnet/standard/base-types/how-to-strip-invalid-characters-from-a-string
        List<Bit16> temp = new();
        string procText = program.Trim().ToUpper();

        if (fromBase == 2)
        {
            procText = Regex.Replace(procText, "[^01]", "");

            if (procText.Length % 16 != 0)
            {
                Console.WriteLine("[!!] WARNING: Program data has extra/lacking bits.");
            }

            for (int i = 0; i + 16 <= procText.Length; i += 16)
            {
                Bit16 line = Convert.ToUInt16(procText.Substring(i, 16), 2);
                temp.Add(line);
            }
        }
        else if (fromBase == 16)
        {
            procText = Regex.Replace(procText, "[^0-9A-F]", "");

            if (procText.Length % 2 != 0)
            {
                Console.WriteLine("[!!] WARNING: Program data has extra/lacking bytes.");
            }

            for (int i = 0; i + 2 <= procText.Length; i += 2)
            {
                Bit16 line = Convert.ToUInt16(procText.Substring(i, 2), 16);
                temp.Add(line);
            }
        }
        else
        {
            throw new ArgumentException("Unsupported base");
        }

        storage = temp.ToArray();

        Console.WriteLine($"[**] ROM16 initialized with {storage.Length} words.");
        // * DEBUG
        for (int i = 0; i < storage.Length; i++)
        {
            Console.WriteLine($"    [{i}] = {storage[i]}");
        }
    }

    public Bit16 Process(Bit16 addr15b, bool clk)
    {
        // Bit16 addr = AND16(addr15b, 0b0111_1111_1111_1111);

        // clk == false から始まった瞬間に出力を更新する
        if (!clk)
        {
            // ! tick の関係上、bitが更新されきらず範囲外にアクセスする可能性がある
            if(addr15b.value < 0 || addr15b.value >= storage.Length)
            {
                Console.WriteLine($"[**] Error: ROM16 - Addr[{addr15b.ToDecString()}] is out of range.");
                output = Bit16Const.FALSE;
            }
            else
            {
                output = storage[addr15b];
            }            
        }

        return output;
    }
}

public class Computerx16
{
    ROM16 instrRom;
    CPUx16 cpu;
    RAM16xVAR ram;

    public Computerx16(ROM16 instrMem, int ramSize = 24577)
    {
        this.instrRom = instrMem;
        cpu = new CPUx16();
        ram = new RAM16xVAR(ramSize);
    }

    public void Process(bool rst, bool clk)
    {
        instrRom.Process(cpu.PCout15, clk);
        cpu.Process(ram.output, instrRom.output, rst, clk);
        ram.Process(cpu.outp, cpu.addrM15, cpu.writeM, clk);
    }

    // * DEBUG
    public void PeekRegisters()
    {
        Console.WriteLine($"instruction ROM Output: {instrRom.output}");
        Console.WriteLine($"ALU Output: {cpu.outp}");
        Console.WriteLine("=== CPU Registers ===");
        Console.WriteLine($"A Register: {cpu.rA.q.ToDecString()}");
        Console.WriteLine($"D Register: {cpu.rD.q.ToDecString()}");
        Console.WriteLine($"PCounter : {cpu.PCout15.ToDecString()}");
    }

    public void RewriteRam(int addr, Bit16 data)
    {
        ram.memoryMaster[addr] = data;
        ram.memorySlave[addr] = data;
    }

    public void PeekRam(int addrStart, int length)
    {
        for (int i = addrStart; i < addrStart + length; i++)
        {
            Console.WriteLine($"RAM[{i}] = {ram.memorySlave[i].ToDecString()}");
        }
    }
}
