public class Parser
{
    Tokenizer _tz;

    public Parser(string inputPath)
    {
        _tz = new Tokenizer(inputPath);
    }

    public Class Compile()
    {
        _tz.Advance();
        var compiledClass = CompileClass();

        return compiledClass;
    }

    // 'class' className '{' classVarDec* subroutineDec* '}'
    Class CompileClass()
    {
        // * consume と同じ処理
        if (_tz.CTokenType != TokenType.KEYWORD || _tz.CKeyword != Keyword.CLASS)
        {
            throw ReportError("Expected 'class' keyword at the beginning of class declaration.");
        }
        _tz.Advance();

        if (_tz.CTokenType != TokenType.IDENTIFIER)
        {
            throw ReportError("Expected class name identifier after 'class' keyword.");
        }
        string className = _tz.CIdentifier;
        _tz.Advance();

        if (_tz.CTokenType != TokenType.SYMBOL || _tz.CSymbol != '{')
        {
            throw ReportError("Expected '{' symbol after class name.");
        }
        _tz.Advance();
        
        List<ClassVarDec> classVarDecs = new();
        while (_tz.CTokenType == TokenType.KEYWORD && (_tz.CKeyword == Keyword.STATIC || _tz.CKeyword == Keyword.FIELD))
        {
            classVarDecs.Add(CompileClassVarDec());
        }

        List<SubroutineDec> subroutineDecs = new();
        while (_tz.CTokenType == TokenType.KEYWORD && (_tz.CKeyword == Keyword.CONSTRUCTOR || _tz.CKeyword == Keyword.FUNCTION || _tz.CKeyword == Keyword.METHOD))
        {
            subroutineDecs.Add(CompileSubroutine());
        }

        if (_tz.CTokenType != TokenType.SYMBOL || _tz.CSymbol != '}')
        {
            throw ReportError("Expected '}' symbol at the end of class declaration.");
        }
        _tz.Advance();

        return new Class(className, new PrintableList<ClassVarDec>(classVarDecs), new PrintableList<SubroutineDec>(subroutineDecs));
    }

    // ('static' | 'field') type varName (',' varName)* ';'
    ClassVarDec CompileClassVarDec()
    {
        ScopeKwd kwd;
        switch (_tz.CKeyword)
        {
            case Keyword.STATIC: kwd = ScopeKwd.STATIC; break;
            case Keyword.FIELD: kwd = ScopeKwd.FIELD; break;
            default: throw ReportError("Expected 'static' or 'field' keyword at the beginning of class variable declaration.");
        }
        _tz.Advance();

        string type;
        if (_tz.CTokenType == TokenType.KEYWORD && (_tz.CKeyword == Keyword.INT || _tz.CKeyword == Keyword.BOOLEAN || _tz.CKeyword == Keyword.CHAR))
        {
            type = _tz.CKeyword.ToString().ToLower();
        }
        else if (_tz.CTokenType == TokenType.IDENTIFIER)
        {
            type = _tz.CIdentifier;
        }
        else
        {
            throw ReportError("Expected type keyword or class name identifier in class variable declaration.");
        }
        _tz.Advance();

        List<string> varNames = new();
        while (true)
        {
            if (_tz.CTokenType != TokenType.IDENTIFIER)
            {
                throw ReportError("Expected variable name identifier in class variable declaration.");
            }
            varNames.Add(_tz.CIdentifier);
            _tz.Advance();

            if (_tz.CTokenType == TokenType.SYMBOL && _tz.CSymbol == ',')
            {
                _tz.Advance();
                continue;
            }
            else if (_tz.CTokenType == TokenType.SYMBOL && _tz.CSymbol == ';')
            {
                _tz.Advance();
                break;
            }
            else
            {
                throw ReportError("Expected ',' or ';' symbol after variable name in class variable declaration.");
            }
        }

        return new ClassVarDec(kwd, type, new PrintableList<string>(varNames));
    }

    // ('constructor' | 'function' | 'method') ('void' | type) subroutineName '(' parameterList ')' subroutineBody
    SubroutineDec CompileSubroutine()
    {
        SubroutineKwd kwd;
        switch (_tz.CKeyword)
        {
            case Keyword.CONSTRUCTOR: kwd = SubroutineKwd.CONSTRUCTOR; break;
            case Keyword.FUNCTION: kwd = SubroutineKwd.FUNCTION; break;
            case Keyword.METHOD: kwd = SubroutineKwd.METHOD; break;
            default: throw ReportError("Expected 'constructor', 'function', or 'method' keyword at the beginning of subroutine declaration.");
        }
        _tz.Advance();

        string returnType;
        if (_tz.CTokenType == TokenType.KEYWORD && _tz.CKeyword == Keyword.VOID)
        {
            returnType = "void";
        }
        else if (_tz.CTokenType == TokenType.KEYWORD && (_tz.CKeyword == Keyword.INT || _tz.CKeyword == Keyword.BOOLEAN || _tz.CKeyword == Keyword.CHAR))
        {
            returnType = _tz.CKeyword.ToString().ToLower();
        }
        else if (_tz.CTokenType == TokenType.IDENTIFIER)
        {
            returnType = _tz.CIdentifier;
        }
        else
        {
            throw ReportError("Expected return type keyword or class name identifier in subroutine declaration.");
        }
        _tz.Advance();

        if (_tz.CTokenType != TokenType.IDENTIFIER)
        {
            throw ReportError("Expected subroutine name identifier in subroutine declaration.");
        }
        string subroutineName = _tz.CIdentifier;
        _tz.Advance();

        if (_tz.CTokenType != TokenType.SYMBOL || _tz.CSymbol != '(')
        {
            throw ReportError("Expected '(' symbol after subroutine name in subroutine declaration.");
        }
        _tz.Advance();

        var parameters = CompileParameterList();

        if (_tz.CTokenType != TokenType.SYMBOL || _tz.CSymbol != ')')
        {
            throw ReportError("Expected ')' symbol after parameter list in subroutine declaration.");
        }
        _tz.Advance();

        var body = CompileSubroutineBody();

        return new SubroutineDec(kwd, returnType, subroutineName, parameters, body);
    }

    // ((type varName) (',' type varName)*)?
    ParameterList CompileParameterList()
    {
        List<(string Type, string Name)> parameters = new();

        while (true)
        {
            string type;
            if (_tz.CTokenType == TokenType.KEYWORD && (_tz.CKeyword == Keyword.INT || _tz.CKeyword == Keyword.BOOLEAN || _tz.CKeyword == Keyword.CHAR))
            {
                type = _tz.CKeyword.ToString().ToLower();
            }
            else if (_tz.CTokenType == TokenType.IDENTIFIER)
            {
                type = _tz.CIdentifier;
            }
            else
            {
                break; // パラメータがない場合
            }
            _tz.Advance();

            if (_tz.CTokenType != TokenType.IDENTIFIER)
            {
                throw ReportError("Expected parameter name identifier in parameter list.");
            }
            string name = _tz.CIdentifier;
            _tz.Advance();

            parameters.Add((type, name));

            if (_tz.CTokenType == TokenType.SYMBOL && _tz.CSymbol == ',')
            {
                _tz.Advance();
                continue;
            }
            else
            {
                break;
            }
        }
        return new ParameterList(new PrintableList<(string Type, string Name)>(parameters));
    }

    // subroutineBody: '{' varDec* statements '}'
    SubroutineBody CompileSubroutineBody()
    {
        if (_tz.CTokenType != TokenType.SYMBOL || _tz.CSymbol != '{')
        {
            throw ReportError("Expected '{' symbol at the beginning of subroutine body.");
        }
        _tz.Advance();

        List<VarDec> varDecs = new();
        while (_tz.CTokenType == TokenType.KEYWORD && _tz.CKeyword == Keyword.VAR)
        {
            varDecs.Add(CompileVarDec());
        }

        var statements = CompileStatements();

        if (_tz.CTokenType != TokenType.SYMBOL || _tz.CSymbol != '}')
        {
            throw ReportError("Expected '}' symbol at the end of subroutine body.");
        }
        _tz.Advance();

        return new SubroutineBody(new PrintableList<VarDec>(varDecs), statements);
    }

    // 'var' type varName (',' varName)* ';'
    VarDec CompileVarDec()
    {
        if (_tz.CTokenType != TokenType.KEYWORD || _tz.CKeyword != Keyword.VAR)
        {
            throw ReportError("Expected 'var' keyword at the beginning of variable declaration.");
        }
        _tz.Advance();

        string type;
        if (_tz.CTokenType == TokenType.KEYWORD && (_tz.CKeyword == Keyword.INT || _tz.CKeyword == Keyword.BOOLEAN || _tz.CKeyword == Keyword.CHAR))
        {
            type = _tz.CKeyword.ToString().ToLower();
        }
        else if (_tz.CTokenType == TokenType.IDENTIFIER)
        {
            type = _tz.CIdentifier;
        }
        else
        {
            throw ReportError("Expected type keyword or class name identifier in variable declaration.");
        }
        _tz.Advance();

        List<string> varNames = new();
        while (true)
        {
            if (_tz.CTokenType != TokenType.IDENTIFIER)
            {
                throw ReportError("Expected variable name identifier in variable declaration.");
            }
            varNames.Add(_tz.CIdentifier);
            _tz.Advance();

            if (_tz.CTokenType == TokenType.SYMBOL && _tz.CSymbol == ',')
            {
                _tz.Advance();
                continue;
            }
            else if (_tz.CTokenType == TokenType.SYMBOL && _tz.CSymbol == ';')
            {
                _tz.Advance();
                break;
            }
            else
            {
                throw ReportError("Expected ',' or ';' symbol after variable name in variable declaration.");
            }
        }

        return new VarDec(type, new PrintableList<string>(varNames));
    }

    // statement*
    PrintableList<Statement> CompileStatements()
    {
        List<Statement> statements = new();
        while (_tz.CTokenType == TokenType.KEYWORD && (_tz.CKeyword == Keyword.LET || _tz.CKeyword == Keyword.IF || _tz.CKeyword == Keyword.WHILE || _tz.CKeyword == Keyword.DO || _tz.CKeyword == Keyword.RETURN))
        {
            switch (_tz.CKeyword)
            {
                case Keyword.LET: statements.Add(CompileLet()); break;
                case Keyword.IF: statements.Add(CompileIf()); break;
                case Keyword.WHILE: statements.Add(CompileWhile()); break;
                case Keyword.DO: statements.Add(CompileDo()); break;
                case Keyword.RETURN: statements.Add(CompileReturn()); break;
            }
        }
        return new PrintableList<Statement>(statements);
    }

    // 'let' varName ('[' expression ']')? '=' expression ';'
    Statement CompileLet()
    {
        if (_tz.CTokenType != TokenType.KEYWORD || _tz.CKeyword != Keyword.LET)
        {
            throw ReportError("Expected 'let' keyword at the beginning of let statement.");
        }
        _tz.Advance();

        if (_tz.CTokenType != TokenType.IDENTIFIER)
        {
            throw ReportError("Expected variable name identifier after 'let' keyword in let statement.");
        }
        string varName = _tz.CIdentifier;
        _tz.Advance();

        Expression? indexExpression = null;
        if (_tz.CTokenType == TokenType.SYMBOL && _tz.CSymbol == '[')
        {
            _tz.Advance();
            indexExpression = CompileExpression();
            if (_tz.CTokenType != TokenType.SYMBOL || _tz.CSymbol != ']')
            {
                throw ReportError("Expected ']' symbol after index expression in let statement.");
            }
            _tz.Advance();
        }

        if (_tz.CTokenType != TokenType.SYMBOL || _tz.CSymbol != '=')
        {
            throw ReportError("Expected '=' symbol after variable name in let statement.");
        }
        _tz.Advance();

        Expression valueExpression = CompileExpression();

        if (_tz.CTokenType != TokenType.SYMBOL || _tz.CSymbol != ';')
        {
            throw ReportError("Expected ';' symbol at the end of let statement.");
        }
        _tz.Advance();

        LetStatement body = new LetStatement(varName, indexExpression, valueExpression);
        return new Statement(StatementKwd.LET, body);
    }

    // 'if' '(' expression ')' '{' statements '}' ('else' '{' statements '}')?
    Statement CompileIf()
    {
        if (_tz.CTokenType != TokenType.KEYWORD || _tz.CKeyword != Keyword.IF)
        {
            throw ReportError("Expected 'if' keyword at the beginning of if statement.");
        }
        _tz.Advance();

        if (_tz.CTokenType != TokenType.SYMBOL || _tz.CSymbol != '(')
        {
            throw ReportError("Expected '(' symbol after 'if' keyword in if statement.");
        }
        _tz.Advance();

        Expression condition = CompileExpression();

        if (_tz.CTokenType != TokenType.SYMBOL || _tz.CSymbol != ')')
        {
            throw ReportError("Expected ')' symbol after condition expression in if statement.");
        }
        _tz.Advance();

        if (_tz.CTokenType != TokenType.SYMBOL || _tz.CSymbol != '{')
        {
            throw ReportError("Expected '{' symbol at the beginning of 'if' statement body.");
        }
        _tz.Advance();

        PrintableList<Statement> thenStatements = CompileStatements();
        if (_tz.CTokenType != TokenType.SYMBOL || _tz.CSymbol != '}')
        {
            throw ReportError("Expected '}' symbol at the end of 'if' statement body.");
        }
        _tz.Advance();

        PrintableList<Statement>? elseStatements = null;
        if (_tz.CTokenType == TokenType.KEYWORD && _tz.CKeyword == Keyword.ELSE)
        {
            _tz.Advance();

            if (_tz.CTokenType != TokenType.SYMBOL || _tz.CSymbol != '{')
            {
                throw ReportError("Expected '{' symbol at the beginning of 'else' statement body.");
            }
            _tz.Advance();

            elseStatements = CompileStatements();
            if (_tz.CTokenType != TokenType.SYMBOL || _tz.CSymbol != '}')
            {
                throw ReportError("Expected '}' symbol at the end of 'else' statement body.");
            }
            _tz.Advance();
        }

        IfStatement body = new IfStatement(condition, thenStatements, elseStatements);
        return new Statement(StatementKwd.IF, body);
    }

    // 'while' '(' expression ')' '{' statements '}'
    Statement CompileWhile()
    {
        if (_tz.CTokenType != TokenType.KEYWORD || _tz.CKeyword != Keyword.WHILE)
        {
            throw ReportError("Expected 'while' keyword at the beginning of while statement.");
        }
        _tz.Advance();

        if (_tz.CTokenType != TokenType.SYMBOL || _tz.CSymbol != '(')
        {
            throw ReportError("Expected '(' symbol after 'while' keyword in while statement.");
        }
        _tz.Advance();

        Expression condition = CompileExpression();

        if (_tz.CTokenType != TokenType.SYMBOL || _tz.CSymbol != ')')
        {
            throw ReportError("Expected ')' symbol after condition expression in while statement.");
        }
        _tz.Advance();

        if (_tz.CTokenType != TokenType.SYMBOL || _tz.CSymbol != '{')
        {
            throw ReportError("Expected '{' symbol at the beginning of while statement body.");
        }
        _tz.Advance();

        PrintableList<Statement> bodyStatements = CompileStatements();

        if (_tz.CTokenType != TokenType.SYMBOL || _tz.CSymbol != '}')
        {
            throw ReportError("Expected '}' symbol at the end of while statement body.");
        }
        _tz.Advance();

        WhileStatement body = new WhileStatement(condition, bodyStatements);
        return new Statement(StatementKwd.WHILE, body);
    }

    // 'do' subroutineCall ';'
    Statement CompileDo()
    {
        if (_tz.CTokenType != TokenType.KEYWORD || _tz.CKeyword != Keyword.DO)
        {
            throw ReportError("Expected 'do' keyword at the beginning of do statement.");
        }
        _tz.Advance();

        string firstIdentifier;
        if (_tz.CTokenType != TokenType.IDENTIFIER)
        {
            throw ReportError("Expected subroutine name identifier after 'do' keyword in do statement.");
        }
        firstIdentifier = _tz.CIdentifier;
        _tz.Advance();

        SubroutineCallTerm call = CompileSubroutineCall(firstIdentifier);

        if (_tz.CTokenType != TokenType.SYMBOL || _tz.CSymbol != ';')
        {
            throw ReportError("Expected ';' symbol at the end of do statement.");
        }
        _tz.Advance();

        DoStatement body = new DoStatement(call);
        return new Statement(StatementKwd.DO, body);
    }

    // 'return' expression? ';'
    Statement CompileReturn()
    {
        if (_tz.CTokenType != TokenType.KEYWORD || _tz.CKeyword != Keyword.RETURN)
        {
            throw ReportError("Expected 'return' keyword at the beginning of return statement.");
        }
        _tz.Advance();

        Expression? returnValue = null;
        if (!(_tz.CTokenType == TokenType.SYMBOL && _tz.CSymbol == ';'))
        {
            returnValue = CompileExpression();
        }
        _tz.Advance();

        ReturnStatement body = new ReturnStatement(returnValue);
        return new Statement(StatementKwd.RETURN, body);
    }

    // term (op term)*
    Expression CompileExpression()
    {
        Term firstTerm = CompileTerm();

        PrintableList<(Operator Op, Term Term)> restTerms = new();

        while (_tz.CTokenType == TokenType.SYMBOL)
        {
            Operator op;
            switch (_tz.CSymbol)
            {
                case '+': op = Operator.ADD; break;
                case '-': op = Operator.SUB; break;
                case '*': op = Operator.MUL; break;
                case '/': op = Operator.DIV; break;
                case '&': op = Operator.AND; break;
                case '|': op = Operator.OR; break;
                case '<': op = Operator.LT; break;
                case '>': op = Operator.GT; break;
                case '=': op = Operator.EQ; break;
                default: return new Expression(firstTerm, restTerms); // 演算子でなければ式の終わり
            }
            _tz.Advance();

            Term term = CompileTerm();
            restTerms.Add((op, term));
        }

        return new Expression(firstTerm, restTerms);
    }

    Term CompileTerm()
    {
        if (_tz.CTokenType == TokenType.INTEGER_CONSTANT)
        {
            int value = _tz.CIntVal;
            _tz.Advance();
            return new IntegerConstant(value);
        }
        else if (_tz.CTokenType == TokenType.STRING_CONSTANT)
        {
            string value = _tz.CStringVal;
            _tz.Advance();
            return new StringConstant(value);
        }
        else if (_tz.CTokenType == TokenType.KEYWORD && (_tz.CKeyword == Keyword.TRUE || _tz.CKeyword == Keyword.FALSE || _tz.CKeyword == Keyword.NULL || _tz.CKeyword == Keyword.THIS))
        {
            ConstKwd value;
            switch (_tz.CKeyword)
            {
                case Keyword.TRUE: value = ConstKwd.TRUE; break;
                case Keyword.FALSE: value = ConstKwd.FALSE; break;
                case Keyword.NULL: value = ConstKwd.NULL; break;
                case Keyword.THIS: value = ConstKwd.THIS; break;
                default: throw ReportError("Unexpected keyword constant in term.");
            }
            _tz.Advance();
            return new KeywordConstant(value);
        }
        // * 先読みが必要な "varName / subroutineName"
        else if (_tz.CTokenType == TokenType.IDENTIFIER)
        {
            string varName = _tz.CIdentifier;
            _tz.Advance();

            // varName '[' expression ']'
            if (_tz.CTokenType == TokenType.SYMBOL && _tz.CSymbol == '[')
            {
                _tz.Advance();
                Expression indexExpression = CompileExpression();
                if (_tz.CTokenType != TokenType.SYMBOL || _tz.CSymbol != ']')
                {
                    throw ReportError("Expected ']' symbol after index expression in term.");
                }
                _tz.Advance();
                return new VarNameIndexTerm(varName, indexExpression);
            }
            // subroutineCall
            else if (_tz.CTokenType == TokenType.SYMBOL && (_tz.CSymbol == '(' || _tz.CSymbol == '.'))
            {
                return CompileSubroutineCall(varName);
            }
            // varName
            else
            {
                return new VarNameTerm(varName);
            }
        }
        else if (_tz.CTokenType == TokenType.SYMBOL && _tz.CSymbol == '(')
        {
            _tz.Advance();
            Expression innerExpression = CompileExpression();
            if (_tz.CTokenType != TokenType.SYMBOL || _tz.CSymbol != ')')
            {
                throw ReportError("Expected ')' symbol after parenthesized expression in term.");
            }
            _tz.Advance();
            return new ParenthesizedTerm(innerExpression);
        }
        else if (_tz.CTokenType == TokenType.SYMBOL && (_tz.CSymbol == '-' || _tz.CSymbol == '~'))
        {
            UnaryOperator op;
            switch (_tz.CSymbol)
            {
                case '-': op = UnaryOperator.NEG; break;
                case '~': op = UnaryOperator.NOT; break;
                default: throw ReportError("Unexpected unary operator symbol in term.");
            }
            _tz.Advance();
            Term innerTerm = CompileTerm();
            return new UnaryOpTerm(op, innerTerm);
        }
        else
        {
            throw ReportError("Unexpected token at the beginning of term.");
        }
    }

    SubroutineCallTerm CompileSubroutineCall(string firstIdentifier)
    {
        if (_tz.CTokenType == TokenType.SYMBOL && _tz.CSymbol == '(')
        {
            // subroutineName '(' expressionList ')'
            _tz.Advance();

            PrintableList<Expression> arguments = CompileExpressionList();

            if (_tz.CTokenType != TokenType.SYMBOL || _tz.CSymbol != ')')
            {
                throw ReportError("Expected ')' symbol after argument list in subroutine call.");
            }
            _tz.Advance();

            return new SubroutineCallTerm(null, firstIdentifier, arguments);
        }
        else if (_tz.CTokenType == TokenType.SYMBOL && _tz.CSymbol == '.')
        {
            // (className | varName) '.' subroutineName '(' expressionList ')'
            string classOrVarName = firstIdentifier;
            _tz.Advance();

            if (_tz.CTokenType != TokenType.IDENTIFIER)
            {
                throw ReportError("Expected subroutine name identifier after '.' symbol in subroutine call.");
            }
            string subroutineName = _tz.CIdentifier;
            _tz.Advance();

            if (_tz.CTokenType != TokenType.SYMBOL || _tz.CSymbol != '(')
            {
                throw ReportError("Expected '(' symbol after subroutine name in subroutine call.");
            }
            _tz.Advance();

            PrintableList<Expression> arguments = CompileExpressionList();

            if (_tz.CTokenType != TokenType.SYMBOL || _tz.CSymbol != ')')
            {
                throw ReportError("Expected ')' symbol after argument list in subroutine call.");
            }
            _tz.Advance();

            return new SubroutineCallTerm(classOrVarName, subroutineName, arguments);
        }
        else
        {
            throw ReportError("Expected '(' or '.' symbol after identifier in subroutine call.");
        }
    }

    // Expression数はList.Countでわかる
    PrintableList<Expression> CompileExpressionList()
    {
        List<Expression> expressions = new();

        if (_tz.CTokenType == TokenType.SYMBOL && _tz.CSymbol == ')')
        {
            return new PrintableList<Expression>(expressions); // 空の引数リスト
        }

        while (true)
        {
            Expression expression = CompileExpression();
            expressions.Add(expression);

            if (_tz.CTokenType == TokenType.SYMBOL && _tz.CSymbol == ',')
            {
                _tz.Advance();
                continue;
            }
            else
            {
                break;
            }
        }

        return new PrintableList<Expression>(expressions);
    }


    SyntaxException ReportError(string message)
    {
        string errorLocation = "Error at token: ";
        
        // エラー箇所から最大8トークン分の情報を取得してエラーメッセージに含める
        for (int i = 0; i < 8 && _tz.HasMoreTokens; i++)
        {
            // ! デバッグ用に現在トークンの生の値を取得できるようにしたほうがいい？
            if (_tz.CTokenType == TokenType.KEYWORD)
            {
                errorLocation += $"{_tz.CKeyword} ";
            }
            else if (_tz.CTokenType == TokenType.SYMBOL)
            {
                errorLocation += $"{_tz.CSymbol} ";
            }
            else if (_tz.CTokenType == TokenType.IDENTIFIER)
            {
                errorLocation += $"{_tz.CIdentifier} ";
            }
            else if (_tz.CTokenType == TokenType.INTEGER_CONSTANT)
            {
                errorLocation += $"{_tz.CIntVal} ";
            }
            else if (_tz.CTokenType == TokenType.STRING_CONSTANT)
            {
                errorLocation += $"{_tz.CStringVal} ";
            }
            _tz.Advance();
        }
        return new SyntaxException($"{message} >> {errorLocation}");
    }
}

// #pragma warning disable CS8618 // nullエラーめんどフェイズ

// 木構造（ノード）をrecordで表現する
// record は ToString() を自動生成してくれるので、デバッグに便利
// == 構造 ==
public record Class(string ClassName, PrintableList<ClassVarDec> ClassVarDecs, PrintableList<SubroutineDec> SubroutineDecs);

public record ClassVarDec(ScopeKwd Kwd, string Type, PrintableList<string> VarNames);
public enum ScopeKwd { STATIC, FIELD }

public record SubroutineDec(SubroutineKwd Kwd, string ReturnType, string SubroutineName, ParameterList Params, SubroutineBody Body);
public enum SubroutineKwd { CONSTRUCTOR, FUNCTION, METHOD }

public record ParameterList(PrintableList<(string Type, string Name)> Params);

public record SubroutineBody(PrintableList<VarDec> VarDecs, PrintableList<Statement> Stmts);

public record VarDec(string Type, PrintableList<string> VarNames);

// == 文 ==
public record Statement(StatementKwd Kwd, BodyStatement Body);
public enum StatementKwd { LET, IF, WHILE, DO, RETURN }

public abstract record BodyStatement { }
public record LetStatement(string VarName, Expression? IndexExpr, Expression ValueExpr) : BodyStatement;
public record IfStatement(Expression Condition, PrintableList<Statement> ThenStmts, PrintableList<Statement>? ElseStmts) : BodyStatement;
public record WhileStatement(Expression Condition, PrintableList<Statement> BodyStmts) : BodyStatement;
public record DoStatement(SubroutineCallTerm CallTerm) : BodyStatement;
public record ReturnStatement(Expression? ReturnExpr) : BodyStatement;

// == 式 ==
public record Expression(Term FirstTerm, PrintableList<(Operator Op, Term Term)> RestTerms);

public abstract record Term { }
public record IntegerConstant(int Value) : Term;
public record StringConstant(string Value) : Term;
public record KeywordConstant(ConstKwd Value) : Term;
public record VarNameTerm(string VarName) : Term;
public record VarNameIndexTerm(string VarName, Expression IndexExpr) : Term;
public record ParenthesizedTerm(Expression InnerExpr) : Term;
public record UnaryOpTerm(UnaryOperator Op, Term InnerTerm) : Term;

public record SubroutineCallTerm(string? ClassOrVarName, string SubroutineName, PrintableList<Expression> Arguments) : Term;

public enum Operator { ADD, SUB, MUL, DIV, AND, OR, LT, GT, EQ }
public enum UnaryOperator { NEG, NOT }
public enum ConstKwd { TRUE, FALSE, NULL, THIS }

// * 構造用リスト
public class PrintableList<T> 
{
    public List<T> Items { get; }


    public PrintableList()
    {
        Items = new List<T>();
    }

    public PrintableList(IEnumerable<T> items)
    {
        Items = items.ToList();
    }

    public void Add(T item)
    {
        Items.Add(item);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    public int Count => Items.Count;

    public T this[int index] => Items[index];

    public override string ToString()
    {
        return $"[{string.Join(", ", Items)}]";
    }
}

public class SyntaxException : Exception
{
    public SyntaxException(string message) : base(message) { }
}
  