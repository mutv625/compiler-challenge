using static BaseGate;

// ! OOP的に回路素子
class NandLatch
{
    public bool q;
    public bool nq;

    public NandLatch()
    {
        q = false;
        nq = true;
    }

    public (bool q, bool nq) process(bool r, bool sw)
    {
        q = !(r & nq);
        nq = !(sw & q);

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

        nl1.process(a, b);
        q = nl1.q;
        nq = nl1.nq;

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


class BaseGate
{
    public static bool AND(bool a, bool b) => a & b;
    public static bool OR(bool a, bool b) => a | b;
    public static bool NOT(bool a) => !a;
    public static bool NAND(bool a, bool b) => !(a & b);
    public static bool NOR(bool a, bool b) => !(a | b);
    public static bool XOR(bool a, bool b) => a ^ b;
}

class SwitchingGate
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
}

class Register
{
    public bool o;
    DFF dff1;

    public Register()
    {
        o = false;
        dff1 = new DFF();
    }


    public bool process(bool d, bool load, bool clk)
    {
        dff1.process(SwitchingGate.Mux2to1(load, o, d), clk);
        o = dff1.q;

        return o;
    }
    
}


class Program
{
    public static void Main()
    {
        DLatch rat = new();

        List<(bool r, bool load)> inputs = new()
        {
            (false, false),
            (true, true),
            (false, false),
            (false, true),
            (true, true),
            (true, false),
            (false, false),
            (false, true),
        };

        // 擬似的にtick
        foreach (var (r, load) in inputs)
        {
            // ! クロックサイクル
            for (int c = 0; c < 2; c++)
            {
                Console.WriteLine($"Clock cycle {c}:");

                // * 動作が安定するまで繰り返す
                for (int t = 0; t < 10; t++)
                {
                    rat.process(r, load);
                }
            }

            Console.WriteLine($"r={r} load={load} => q={rat.q} \n");
        }
    }
}