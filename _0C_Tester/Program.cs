using System.Diagnostics;

using static BasicGates.Bit.BooleanLogic;
using static BasicGates.Bit.SwitchingGate;

using BasicGates.x16;
using static BasicGates.x16.BooleanLogic16;
using Computing.x16;

class Program
{
    const int MAX_CLOCK_COUNT = 300;
    const int MAX_TICK_COUNT = 8;

    const string bintxt = @"
0000000000000010
1110101010001000
0000000000000000
1111110000010000
0000000000010000
1110001100001000
0000000000010000
1111110000010000
0000000000010100
1110001100000010
0000000000000010
1111110000010000
0000000000000001
1111000010010000
0000000000000010
1110001100001000
0000000000010000
1111110010001000
0000000000000110
1110101010000111
0000000000010100
1110101010000111
    ";
        

    static void Main(string[] args)
    {
        Stopwatch sw = new Stopwatch(); sw.Start();

        // * 入力ケース
        // string globalInputs = @"";

        // * 出力の要件


        // ! 素子の宣言
        ROM16 instructionROM = new ROM16(bintxt, 2);


        Computerx16 computer = new Computerx16(instructionROM, 32768);

        computer.RewriteRam(0, 9);
        computer.RewriteRam(1, 7);
        computer.RewriteRam(2, 100);


        // Clock loop
        for (int c = 0; c < MAX_CLOCK_COUNT; c++)
        {
            // var gin = globalInputs[c];

            // * Clock phase
            for (int p = 0; p <= 1; p++)
            {
                bool clk = p == 1;
                // Tick loop
                for (int t = 0; t < MAX_TICK_COUNT; t++)
                {
                    // ! ENTRYPOINT
                    computer.Process(false, clk);

                    // ! ENTRYPOINT END


                    // * DEBUG

                    if (t == MAX_TICK_COUNT - 1 && p == 1)
                    {
                        Console.WriteLine($"C={c} P={p} T={t} => ");
                        computer.PeekRam(0, 3);
                        computer.PeekRam(16, 1);
                        computer.PeekRegisters();
                        Console.WriteLine("-------------------------");
                    }
                }
            }
        }

        sw.Stop();
        Console.WriteLine($"Elapsed = {sw.Elapsed.TotalNanoseconds / 1000000} ms");
    }
}
