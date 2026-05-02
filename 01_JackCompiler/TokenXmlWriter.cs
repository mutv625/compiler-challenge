public class TokenXmlWriter
{
    string _outPath;

    StreamWriter _writer;

    public TokenXmlWriter(string outPath)
    {
        _outPath = outPath;
        _writer = new StreamWriter(_outPath);
        _writer.WriteLine("<tokens>");
    }

    public void Close()
    {
        _writer.WriteLine("</tokens>");
        _writer.Close();
    }

    public void WriteLine(TokenType type, string val)
    {
        if (type == TokenType.EOF) return;
        _writer.WriteLine($"<{type.ToLowwerCamelCase()}> {val.OnXml()} </{type.ToLowwerCamelCase()}>");
    }
}

// # 拡張メソッド
// static class 中に、 第一引数に this キーワードを修飾子として付けた static メソッドを書くらしい
static class TokenTypeExtension
{
    public static string ToLowwerCamelCase(this TokenType type)
    {
        string s = type.ToString();
        string[] parts = s.Split('_');
        
        for (int i = 0; i < parts.Length; i++)
        {
            if (i == 0)
            {
                parts[0] = parts[0].ToLower();
            }
            else
            {
                parts[i] = Char.ToUpper(parts[i][0]) + parts[i].Substring(1).ToLower();
            }
        }

        return String.Join("", parts);
    }
}

static class StringExtension
{
    public static string OnXml(this string str)
    {
        return str.Replace("&", "&amp;")
                  .Replace("<", "&lt;")
                  .Replace(">", "&gt;")
                  .Replace("\"", "&quot;")
                  .Replace("'", "&apos;");
    }
}
