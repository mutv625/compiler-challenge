public enum TokenType
{
    TK_NUM,
    TK_ADD,
    TK_MUL,
    TK_PAREN_L,
    TK_PAREN_R,
    TK_EOF,
}


public class Token
{
    public TokenType Type { get; }
    public string Value { get; }

    public Token(TokenType type, string value)
    {
        Type = type;
        Value = value;
    }

    public override string ToString() => $"{Type}: '{Value}'";
}

public class ArithmTokenizer
{
    public ArithmTokenizer(string input)
    {
        _input = input;
    }

    private readonly string _input;
    /// <summary>
    /// ReadChar()によって読まれる現在のカーソル位置。
    /// </summary>
    private int _currInputPos = 0;

    private List<Token> _tokens = new();

    /// <summary>
    /// 直前にReadToken()によって生成されたトークン。ReadToken()を呼び出すたびに更新される。
    /// </summary>
    /// <value></value>
    public Token? GeneratedToken { get; private set; }


    public void ReadToken()
    {
        if (_currInputPos == _input.Length)
        {
            RegisterToken(new Token(TokenType.TK_EOF, ""));
            return;
        }

        if (_currInputPos > _input.Length)
        {
            throw new Exception("Read position exceeded input length.");
        }


        char currentChar = _input[_currInputPos];
        
        if (char.IsWhiteSpace(currentChar))
        {
            while (_currInputPos < _input.Length && char.IsWhiteSpace(_input[_currInputPos]))
            {
                _currInputPos++;
            }
            currentChar = _input[_currInputPos];
        }

        if (char.IsDigit(currentChar))
        {
            int len = 0;
            while (_currInputPos + len < _input.Length && char.IsDigit(_input[_currInputPos + len]))
            {
                len++;
            } 

            RegisterToken(new Token(TokenType.TK_NUM, _input.Substring(_currInputPos, len)));
            _currInputPos += len;
        }
        else if (currentChar == '+' || currentChar == '-')
        {
            RegisterToken(new Token(TokenType.TK_ADD, currentChar.ToString()));
            _currInputPos++;
        }
        else if (currentChar == '*' || currentChar == '/')
        {
            RegisterToken(new Token(TokenType.TK_MUL, currentChar.ToString()));
            _currInputPos++;
        }
        else if (currentChar == '(')
        {
            RegisterToken(new Token(TokenType.TK_PAREN_L, currentChar.ToString()));
            _currInputPos++;
        }
        else if (currentChar == ')')
        {
            RegisterToken(new Token(TokenType.TK_PAREN_R, currentChar.ToString()));
            _currInputPos++;
        }
        else
        {
            throw new Exception($"Unexpected character: '{currentChar}' at position {_currInputPos}");
        }
    }

    private void RegisterToken(Token token)
    {
        _tokens.Add(token);
        GeneratedToken = token;
    }

}