using System.Text;

public class Tokenizer
{
    string _loadedStr;

    public Tokenizer(string filePath)
    {
        _loadedStr = File.ReadAllText(filePath);
        HasMoreTokens = true;
    }

    public bool HasMoreTokens { get; private set; }

    public TokenType CTokenType { get; private set; }

    
    public Keyword CKeyword { get { if (CTokenType != TokenType.KEYWORD) throw new TokenizeException("Current token is not a keyword."); else return field; } private set; }

    public char CSymbol { get { if (CTokenType != TokenType.SYMBOL) throw new TokenizeException("Current token is not a symbol."); else return field; } private set; }

    public string CIdentifier { get { if (CTokenType != TokenType.IDENTIFIER) throw new TokenizeException("Current token is not an identifier."); else return field; } private set; } = "";

    public int CIntVal { get { if (CTokenType != TokenType.INTEGER_CONSTANT) throw new TokenizeException("Current token is not an integer constant."); else return field; } private set; }

    public string CStringVal { get { if (CTokenType != TokenType.STRING_CONSTANT) throw new TokenizeException("Current token is not a string constant."); else return field; } private set; } = "";


    int pos = 0;

    static char[] symbolList = {'{', '}', '(', ')', '[', ']', '.', ',', ';', '+', '-', '*', '/', '&', '|', '<', '>', '=', '~'};

    public void Advance()
    {
        if (!HasMoreTokens)
        {
            throw new TokenizeException("No token left.");
        }
        
        // # 終点判定
        if (pos >= _loadedStr.Length)
        {
            HasMoreTokens = false;
            CTokenType = TokenType.EOF;
            return;
        }

        // # 空白スキップ
        if (Char.IsWhiteSpace(_loadedStr[pos]))
        {
            while (Char.IsWhiteSpace(_loadedStr[pos]))
            {
                pos++;

                if (pos >= _loadedStr.Length)
                {
                    HasMoreTokens = false;
                    return;
                }
            } 
        }

        // # コメントスキップ
        if (_loadedStr[pos] == '/' && pos + 1 < _loadedStr.Length)
        {
            if (_loadedStr[pos + 1] == '/')
            {
                pos += 2;
                while (pos < _loadedStr.Length && _loadedStr[pos] != '\n')
                {
                    pos++;
                }
                Advance();
                return;
            }
            else if (_loadedStr[pos + 1] == '*')
            {
                pos += 2;
                while (pos + 1 < _loadedStr.Length && !(_loadedStr[pos] == '*' && _loadedStr[pos + 1] == '/'))
                {
                    pos++;
                }
                pos += 2;   // skip '*/'
                Advance();
                return;
            }
        }

        // # 整数定数
        if (Char.IsNumber(_loadedStr[pos]))
        {
            for (int i = pos; i < _loadedStr.Length; i++)
            {
                if (!Char.IsNumber(_loadedStr[i]))
                {
                    CTokenType = TokenType.INTEGER_CONSTANT;
                    CIntVal = int.Parse(_loadedStr.Substring(pos, i - pos));

                    pos = i;
                    return;
                }
            }
            // EOF 処理
            CTokenType = TokenType.INTEGER_CONSTANT;
            CIntVal = int.Parse(_loadedStr.Substring(pos, _loadedStr.Length - pos));
            pos = _loadedStr.Length;
            return;
        }

        // # シンボル
        if (symbolList.Contains(_loadedStr[pos]))
        {
            CTokenType = TokenType.SYMBOL;
            CSymbol = _loadedStr[pos];

            pos++;
            return;
        }

        // # 文字列定数
        if (_loadedStr[pos] == '"')
        {
            for (int i = pos + 1; i < _loadedStr.Length; i++)
            {
                if (_loadedStr[i] == '"')
                {
                    CTokenType = TokenType.STRING_CONSTANT;
                    CStringVal = _loadedStr.Substring(pos + 1, i - pos - 1);

                    pos = i + 1;
                    return;
                }
            }
            // EOF 処理
            throw new TokenizeException("Unterminated string constant.");
        }

        //# 識別子 or キーワード (_ を許可)
        if (Char.IsLetter(_loadedStr[pos]) || _loadedStr[pos] == '_')
        {
            for (int i = pos; i < _loadedStr.Length; i++)
            {
                if (!Char.IsLetterOrDigit(_loadedStr[i]) && _loadedStr[i] != '_')
                {
                    string word = _loadedStr.Substring(pos, i - pos);

                    if (Enum.TryParse(word.ToUpper(), out Keyword keyword))
                    {
                        CTokenType = TokenType.KEYWORD;
                        CKeyword = keyword;
                    }
                    else
                    {
                        CTokenType = TokenType.IDENTIFIER;
                        CIdentifier = word;
                    }

                    pos = i;
                    return;
                }
            }
            // reached EOF while reading identifier
            string wordAtEof = _loadedStr.Substring(pos, _loadedStr.Length - pos);
            if (Enum.TryParse(wordAtEof.ToUpper(), out Keyword kwEof))
            {
                CTokenType = TokenType.KEYWORD;
                CKeyword = kwEof;
            }
            else
            {
                CTokenType = TokenType.IDENTIFIER;
                CIdentifier = wordAtEof;
            }
            pos = _loadedStr.Length;
            return;
        }

        // # ここまで来たら不正なトークン
        throw new TokenizeException($"Invalid character at position {pos}");
    }
}

public enum TokenType
{
    KEYWORD,
    SYMBOL,
    IDENTIFIER,
    INTEGER_CONSTANT,
    STRING_CONSTANT,
    EOF
}

public enum Keyword
{
    CLASS,
    METHOD,
    FUNCTION,
    CONSTRUCTOR,
    INT,
    BOOLEAN,
    CHAR,
    VOID,
    VAR,
    STATIC,
    FIELD,
    LET,
    DO,
    IF,
    ELSE,
    WHILE,
    RETURN,
    TRUE,
    FALSE,
    NULL,
    THIS
}

public class TokenizeException : Exception
{
    public TokenizeException(string message) : base(message) { }
}