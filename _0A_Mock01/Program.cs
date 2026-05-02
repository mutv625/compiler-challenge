using System;
using System.Collections.Generic;
using System.Diagnostics;

class Program
{
    const int MAX_CLOCK_COUNT = 8;
    const int MAX_TICK_COUNT = 10;


    static void Main(string[] args)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        // * 入力ケース
        List<(bool a, bool b, bool c)> globalInputs = [
            (false, false, false),
            (false, false, true),
            (false, true, false),
            (false, true, true),
            (true, false, false),
            (true, false, true),
            (true, true, false),
            (true, true, true),
        ];
        var globalInput = globalInputs[0];

        // * 出力の要件
        (bool x, bool y) globalOutput;


        bool clock = false;
        // Clock loop
        for (int c = 0; c < MAX_CLOCK_COUNT; c++)
        {
            globalInput = globalInputs[c];

            for (int i = 0; i <= 1; i++)
            {
                // Tick loop
                for (int t = 0; t < MAX_TICK_COUNT; t++)
                {
                    // ! ENTRYPOINT
                    globalOutput = FADD(globalInput.a, globalInput.b, globalInput.c);
                    // ! ENTRYPOINT END


                    // * DEBUG
                    if (clock == true && t == MAX_TICK_COUNT - 1)
                    Console.WriteLine($"Clock: {clock}, tick:{t}; Input: {globalInput}, Output: {globalOutput}");
                }

                clock = !clock;
            }
        }

        sw.Stop();
        Console.WriteLine($"Elapsed = {sw.Elapsed.TotalNanoseconds/1000000} ms");
    }

    static bool NAND(bool a, bool b)
    {
        return !(a & b);
    }

    static bool NOT(bool x)
    {
        return NAND(x, x);
    }

    static bool AND(bool a, bool b)
    {
        return NOT(NAND(a, b));
    }

    static bool OR(bool a, bool b)
    {
        return NAND(NOT(a), NOT(b));
    }

    static bool XOR(bool a, bool b)
    {
        return AND(OR(a, b), NAND(a, b));
    }

    static (bool car, bool sum) HADD(bool a, bool b)
    {
        return (AND(a, b), XOR(a, b));
    }

    static (bool car, bool sum) FADD(bool a, bool b, bool cin)
    {
        var (car1, sum1) = HADD(a, b);
        var (car2, sum2) = HADD(sum1, cin);
        var car = OR(car1, car2);
        return (car, sum2);
    }

    static (bool q, bool nq) DFF(bool r, bool s)
    {
        throw new NotImplementedException();
    }


}