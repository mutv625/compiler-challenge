using static BasicGates.Bit.BooleanLogic;
using static BasicGates.Bit.SwitchingGate;

namespace BasicGates.Bit;

public static class BooleanLogic
{
    public static bool NAND(bool a, bool b)
    {
        return !(a & b);
    }

    public static bool NOT(bool a)
    {
        return NAND(a, a);
    }

    public static bool AND(bool a, bool b)
    {
        return NOT(NAND(a, b));
    }

    public static bool OR(bool a, bool b)
    {
        return NAND(NOT(a), NOT(b));
    }

    public static bool XOR(bool a, bool b)
    {
        return AND(OR(a, b), NAND(a, b));
    }

    public static bool XNOR(bool a, bool b)
    {
        return NOT(XOR(a, b));
    }
}

public static class SwitchingGate
{
    public static bool Mux2to1(bool sel, bool a0, bool b1)
    {
        return OR(AND(NOT(sel), a0), AND(sel, b1));
    }

    public static (bool q0, bool q1) Demux1to2(bool sel, bool v)
    {
        bool q0 = AND(NOT(sel), v);
        bool q1 = AND(sel, v);
        return (q0, q1);
    }

    public static (bool q0, bool q1, bool q2, bool q3) Demux1to4((bool s2, bool s1) sel, bool i)
    {
        (bool a0, bool a1) = Demux1to2(sel.s2, i);
        (bool q0, bool q1) = Demux1to2(sel.s1, a0);
        (bool q2, bool q3) = Demux1to2(sel.s1, a1);
        return (q0, q1, q2, q3);
    }
}


class NandLatch
{
    public bool q;
    public bool nq;

    public NandLatch()
    {
        q = false;
        nq = true;
    }

    public (bool q, bool nq) process(bool r, bool s)
    {
        q = !(r & nq);
        nq = !(s & q);

        return (q, nq);
    }
}

class DLatch
{
    public bool q;
    public bool nq;

    NandLatch nl1;

    public DLatch()
    {
        q = false;
        nq = true;
        nl1 = new();
    }

    public (bool q, bool nq) process(bool r, bool sw)
    {
        bool a = !(r & sw);
        bool b = !(a & sw);

        (q, nq) = nl1.process(a, b);        

        return (q, nq);
    }
}

class DFF
{
    public bool q;

    DLatch db1;
    DLatch db2;

    public DFF()
    {
        q = false;
        db1 = new();
        db2 = new();
    }

    public bool process(bool d, bool clk)
    {
        db1.process(d, clk);
        db2.process(db1.q, !clk);

        q = db2.q;

        return q;
    }
}

class Register
{
    public bool q;
    DFF dff1;

    public Register()
    {
        q = false;
        dff1 = new DFF();
    }


    public bool process(bool d, bool load, bool clk)
    {
        dff1.process(Mux2to1(load, q, d), clk);
        q = dff1.q;

        return q;
    }
    
}
    