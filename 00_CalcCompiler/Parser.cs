// # 親ノード
public class Expr
{
}

// # 数字ノード（終端）(<number> =:: <factor>)
public sealed class NumberExpr : Expr
{
    public int Value { get; }

    public NumberExpr(int value)
    {
        Value = value;
    }
}

// # 単項演算ノード
public sealed class UnaryExpr : Expr
{
    public TokenType OpTokenType { get; }
    public string Operator { get; }
    // * 子被演算ノード
    public Expr Operand { get; }

    public UnaryExpr(TokenType op, string opStr, Expr operand)
    {
        OpTokenType = op;
        Operator = opStr;
        Operand = operand;
    }
}

// # 二項演算ノード
public sealed class BinaryExpr : Expr
{
    public TokenType OpTokenType { get; }
    public string Operator { get; }
    public Expr Left { get; }
    public Expr Right { get; }

    public BinaryExpr(TokenType op, string opStr, Expr left, Expr right)
    {
        OpTokenType = op;
        Operator = opStr;
        Left = left;
        Right = right;
    }
}

public class ArithmParser
{
    public ArithmParser(List<Token> tokens)
    {
        _tokens = tokens;
    }

    private readonly List<Token> _tokens;
    private int _currTokenIndex = 0;

    private Token Current => _tokens[_currTokenIndex];

    private void MoveNext()
    {
        if (_currTokenIndex < _tokens.Count - 1)
        {
            _currTokenIndex++;
        }
    }

    /// <summary>
    /// 現在のトークンが期待したタイプかを検証し、消費する
    /// </summary>
    private Token Expect(TokenType expectedType)
    {
        if (Current.Type != expectedType)
        {
            throw new Exception($"Expected {expectedType}, but got {Current.Type} : {Current.Value}.");
        }

        Token token = Current;
        MoveNext();
        return token;
    }

    public Expr ParseParent()
    {
        Expr expr = ParseExpr();
        Expect(TokenType.TK_EOF);

        return expr;
    }

    // <expr> ::= <term> (<TK_ADD> <term>)*
    private Expr ParseExpr()
    {   
        bool hasMinus = false;
        if (Current.Type == TokenType.TK_ADD && Current.Value == "-")
        {
            MoveNext();
            hasMinus = true;
        }

        // <term>
        Expr left = ParseTerm();

        // (<TK_ADD> <term>)*
        while (Current.Type == TokenType.TK_ADD)
        {
            TokenType op = Current.Type;
            string opStr = Current.Value;
            MoveNext();

            Expr right = ParseTerm();

            left = new BinaryExpr(op, opStr, left, right);
        }

        return hasMinus ? new UnaryExpr(TokenType.TK_ADD, "-", left): left;
    }

    private Expr ParseTerm()
    {
        // <factor>
        Expr left = ParseFactor();

        // (<TK_MUL> <factor>)*
        while (Current.Type == TokenType.TK_MUL)
        {
            TokenType op = Current.Type;
            string opStr = Current.Value;
            MoveNext();

            Expr right = ParseFactor();

            left = new BinaryExpr(op, opStr, left, right);
        }

        return left;
    }

    private Expr ParseFactor()
    {
        // <number>
        if (Current.Type == TokenType.TK_NUM)
        {
            int value = int.Parse(Current.Value);
            MoveNext();
            return new NumberExpr(value);
        }
        else
        {
            // '(' <expr> ')'
            Expect(TokenType.TK_PAREN_L);
            Expr inner = ParseExpr();
            Expect(TokenType.TK_PAREN_R);

            return inner;
        }
    }
}


public static class AstPrinter
{
    public static void Print(Expr expr, string indent = "")
    {
        switch (expr)
        {
            case NumberExpr number:
                Console.WriteLine($"{indent}Number({number.Value})");
                break;

            case UnaryExpr unary:
                Console.WriteLine($"{indent}Unary({unary.Operator})");
                Print(unary.Operand, indent + "  ");
                break;

            case BinaryExpr binary:
                Console.WriteLine($"{indent}Binary({binary.Operator})");
                Print(binary.Left, indent + "  ");
                Print(binary.Right, indent + "  ");
                break;

            default:
                throw new Exception("Unknown expression node.");
        }
    }
}
