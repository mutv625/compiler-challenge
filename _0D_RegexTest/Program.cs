using System.Text.RegularExpressions;

string input =@"
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
1110101010000111";

Console.WriteLine(String.Join(", ", ROM16(input, 2)));


short[] ROM16(string program, int fromBase)
{
    // ref : https://learn.microsoft.com/ja-jp/dotnet/standard/base-types/how-to-strip-invalid-characters-from-a-string
    List<short> temp = new();
    string procText = program.Trim().ToUpper();

    if (fromBase == 2)
    {
        procText = Regex.Replace(procText, "[^01]", "");

        if (procText.Length % 16 != 0)
        {
            Console.WriteLine("[!!] WARNING: Program data has extra/lacking bits");
        }

        for (int i = 0; i + 16 <= procText.Length; i += 16)
        {
            short line = Convert.ToInt16(procText.Substring(i, 16), 2);
            temp.Add(line);
        }
    }
    else if (fromBase == 16)
    {
        procText = Regex.Replace(procText, "[^0-9A-F]", "");

        if (procText.Length % 2 != 0)
        {
            Console.WriteLine("[!!] WARNING: Program data has extra/lacking bytes");
        }

        for (int i = 0; i + 2 <= procText.Length; i += 2)
        {
            short line = Convert.ToInt16(procText.Substring(i, 2), 16);
            temp.Add(line);
        }
    }
    else
    {
        throw new ArgumentException("Unsupported base");
    }

    return temp.ToArray();
}
