using System;
using BasicGates.Bit;
using static BasicGates.x16.BooleanLogic16;
using static BasicGates.x16.SwitchingGate16;

namespace BasicGates.x16;

public struct Bit16
{
    public ushort value { get; set; }

    public bool this[int index]
    {
        get
        {
            if (index < 0 || index >= 16)
                throw new IndexOutOfRangeException();
            return (value & (1 << index)) != 0;
        }
        set
        {
            if (index < 0 || index >= 16)
                throw new IndexOutOfRangeException();
            if (value)
                this.value |= (ushort)(1 << index);
            else
                this.value &= (ushort)~(1 << index);
        }
    }

    public static implicit operator ushort(Bit16 b) => b.value;
    public static implicit operator Bit16(ushort v) => new Bit16(v);

    /// <summary>
    /// bool から Bit16 へは明示的に、全ビットへの変換をします。
    /// false は 0x0000、true は 0xFFFF に変換されます。
    /// </summary>
    /// <param name="b"></param>
    public static explicit operator Bit16(bool b)
    {
        return b ? new Bit16(0xFFFF) : new Bit16(0x0000);
    }

    public Bit16(ushort value)
    {
        this.value = value;
    }

    public override string ToString()
    {
        string ret = value.ToString("B16");
        for (int i = 4; i < ret.Length; i += 5)
        {
            ret = ret.Insert(i, "_");
        }
        ret = "0b" + ret.TrimEnd();
        return ret;
    }

    public string ToHexString()
    {
        string ret = "0x" + value.ToString("X4");
        for (int i = 4; i < ret.Length; i += 3)
        {
            ret = ret.Insert(i, " ");
        }
        ret = ret.TrimEnd();
        return ret;
    }

    public string ToUDecString()
    {
        return value.ToString();
    }

    public string ToDecString()
    {
        short sval = (short)value;
        return sval.ToString();
    }
}

public static class Bit16Const
{
    public static readonly Bit16 FALSE = (Bit16)0x0000;
    public static readonly Bit16 ONE = (Bit16)0x0001;
    public static readonly Bit16 TRUE = (Bit16)0xFFFF;
}

/// <summary>
/// 16bit用の基本論理ゲート。各ビットに対してビット単位で論理演算を行います。
/// </summary>
public static class BooleanLogic16
{
    public static Bit16 NAND16(Bit16 a, Bit16 b)
    {
        return new Bit16((ushort)~(a.value & b.value));
    }

    public static Bit16 NOT16(Bit16 a)
    {
        return new Bit16((ushort)~a.value);
    }

    public static Bit16 AND16(Bit16 a, Bit16 b)
    {
        return new Bit16((ushort)(a.value & b.value));
    }

    public static Bit16 OR16(Bit16 a, Bit16 b)
    {
        return new Bit16((ushort)(a.value | b.value));
    }

    public static Bit16 XOR16(Bit16 a, Bit16 b)
    {
        return new Bit16((ushort)(a.value ^ b.value));
    }
}

public static class SwitchingGate16
{
    public static Bit16 Mux2to1_16(bool sel, Bit16 a0, Bit16 b1)
    {
        return OR16(AND16(NOT16((Bit16)sel), a0), AND16((Bit16)sel, b1));
    }

    public static (Bit16 q0, Bit16 q1) Demux1to2_16(bool sel, Bit16 v)
    {
        Bit16 notSel = NOT16((Bit16)sel);
        Bit16 q0 = AND16(notSel, v);
        Bit16 q1 = AND16((Bit16)sel, v);
        return (q0, q1);
    }
}

class NandLatch16
{
    public Bit16 q;
    public Bit16 nq;

    public NandLatch16()
    {
        q = Bit16Const.FALSE;
        nq = Bit16Const.TRUE;
    }

    public (Bit16 q, Bit16 nq) process(Bit16 r, Bit16 s)
    {
        q = NOT16(AND16(r, nq));
        nq = NOT16(AND16(s, q));

        return (q, nq);
    }
}

// ! clk == true のときに load する
class DLatch16
{
    public Bit16 q;
    public Bit16 nq;

    NandLatch16 nl16;

    public DLatch16()
    {
        q = Bit16Const.FALSE;
        nq = Bit16Const.TRUE;
        nl16 = new NandLatch16();
    }

    public (Bit16 q, Bit16 nq) process(Bit16 d, bool clk)
    {
        Bit16 a = NAND16(d, (Bit16)clk);
        Bit16 b = NAND16(a, (Bit16)clk);

        (q, nq) = nl16.process(a, b);

        return (q, nq);
    }
}

class DFF16
{
    public Bit16 q;

    DLatch16 dl16;
    DLatch16 dl16_sub;

    public DFF16()
    {
        q = Bit16Const.FALSE;
        dl16 = new DLatch16();
        dl16_sub = new DLatch16();
    }

    public Bit16 process(Bit16 d, bool clk)
    {
        // Master-Slave DFF
        (Bit16 q1, _) = dl16.process(d, clk);
        (q, _) = dl16_sub.process(q1, !clk);

        return q;
    }
}

public class Register16
{
    public Bit16 q;
    DFF16 dff16;

    public Register16()
    {
        q = Bit16Const.FALSE;
        dff16 = new DFF16();
    }

    public Bit16 process(Bit16 d, bool load, bool clk)
    {
        Bit16 din = Mux2to1_16(load, q, d);
        q = dff16.process(din, clk);
        return q;
    }
}

// TODO: ちょっとズル技を使っているのでレポートで修正
class RAM16x16
{
    public Bit16 output; // 読み出し中の値
    public Register16[] registers;

    public RAM16x16()
    {
        output = Bit16Const.FALSE;

        registers = new Register16[16];
        for (int i = 0; i < 16; i++)
        {
            registers[i] = new Register16();
        }
    }

    public Bit16 process(Bit16 d, Bit16 addr4b, bool load, bool clk)
    {
        int outSel = AND16(addr4b.value, 0x000F); // 下位4bit

        for (int i = 0; i < 16; i++)
        {
            registers[i].process(d, BooleanLogic.AND(load, i == outSel), clk);
        }
        // registers[i].q に各レジスタの値が入る
        output = registers[outSel].q;

        // 出力はアドレスで選択されたレジスタの値
        return output;
    }

    // public Bit16 Mux16to1_16(Bit16 sel, Bit16[] inputs)
    // {
    //     int outSel = sel.value & 0x000F; // 下位4bit
    //     if (outSel < 0 || outSel >= inputs.Length)
    //     {
    //         System.Console.WriteLine("[!!] Error: Mux16to1_16 - Selection out of range.");
    //         return Bit16Const.FALSE;
    //     }

    //     // 本当はセレクタも基本回路などから構築するべきだが、
    //     // ここでは簡略化のために直接インデックス指定で取得する
    //     return inputs[outSel];
    // }
}

class RAM16x256
{
    public Bit16 output; // 読み出し中の値
    public RAM16x16[] ramBlocks;

    public RAM16x256()
    {
        output = Bit16Const.FALSE;

        ramBlocks = new RAM16x16[16];
        for (int i = 0; i < 16; i++)
        {
            ramBlocks[i] = new RAM16x16();
        }
    }

    public Bit16 process(Bit16 d, Bit16 addr8b, bool load, bool clk)
    {
        int blockSel = (addr8b.value >> 4) & 0x000F; // 上位4bit
        Bit16 subAddr = (Bit16)(addr8b.value & 0x000F); // 下位4bit

        for (int i = 0; i < 16; i++)
        {
            ramBlocks[i].process(d, subAddr, BooleanLogic.AND(load, i == blockSel), clk);
        }
        // ramBlocks[i].registers に各RAMブロックのレジスタが入る
        output = ramBlocks[blockSel].output;

        // 出力はアドレスで選択されたRAMブロックの出力
        return output;
    }
}

// 1回の読み書きに40msかかる……
class RAM16x4k
{
    public Bit16 output; // 読み出し中の値
    public RAM16x256[] ramModules;

    public RAM16x4k()
    {
        output = Bit16Const.FALSE;

        ramModules = new RAM16x256[16];
        for (int i = 0; i < 16; i++)
        {
            ramModules[i] = new RAM16x256();
        }
    }

    public Bit16 process(Bit16 d, Bit16 addr12b, bool load, bool clk)
    {
        int moduleSel = (addr12b.value >> 8) & 0x000F; // 上位4bit
        Bit16 subAddr = (Bit16)(addr12b.value & 0x00FF); // 下位8bit

        for (int i = 0; i < 16; i++)
        {
            ramModules[i].process(d, subAddr, BooleanLogic.AND(load, i == moduleSel), clk);
        }
        // ramModules[i].output に各RAMモジュールの出力が入る
        output = ramModules[moduleSel].output;

        // 出力はアドレスで選択されたRAMモジュールの出力
        return output;
    }
}

// 流石に配列で擬似再現
class RAM16x16k
{
    public Bit16 output; // 読み出し中の値
    public Bit16[] memorySlave = new Bit16[16384];
    public Bit16[] memoryMaster = new Bit16[16384];

    public RAM16x16k()
    {
        output = Bit16Const.FALSE;

        for (int i = 0; i < 16384; i++)
        {
            memorySlave[i] = Bit16Const.FALSE;
            memoryMaster[i] = Bit16Const.FALSE;
        }
    }

    public Bit16 process(Bit16 d, Bit16 addr14b, bool load, bool clk)
    {
        ushort addr = AND16(addr14b.value, 0x3FFF); // 下位14bit

        // 書き込みはマスターに対して行う
        if (clk && load)
        {
            memoryMaster[addr] = d;
        }

        // マスターの内容をスレーブにコピーする
        if (!clk)
        {
            for (int i = 0; i < 16384; i++)
            {
                memorySlave[i] = memoryMaster[i];
            }
        }

        // 読み出しはスレーブから行う
        output = memorySlave[addr];

        return output;
    }


}

// 自由メモリサイズで擬似再現 (本では:24577)
class RAM16xVAR
{
    public Bit16 output; // 読み出し中の値
    public Bit16[] memoryMaster;
    public Bit16[] memorySlave;


    public readonly int size;

    public RAM16xVAR(int size)
    {
        this.size = size;
        output = Bit16Const.FALSE;

        memorySlave = new Bit16[size];
        memoryMaster = new Bit16[size];

        for (int i = 0; i < size; i++)
        {
            memorySlave[i] = Bit16Const.FALSE;
            memoryMaster[i] = Bit16Const.FALSE;
        }
    }

    public Bit16 Process(Bit16 d, Bit16 addr15b, bool load, bool clk)
    {
        ushort addr = AND16(addr15b.value, 0x7FFF); // 下位15bit

        // 書き込みはマスターに対して行う
        if (clk && load)
        {
            memoryMaster[addr] = d;
        }

        // マスターの内容をスレーブにコピーする
        if (!clk)
        {
            for (int i = 0; i < size; i++)
            {
                memorySlave[i] = memoryMaster[i];
            }
        }

        // 読み出しはスレーブから行う
        output = memorySlave[addr];

        return output;
    }
}

