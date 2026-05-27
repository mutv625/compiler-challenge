using System.Text;
using static ClassSymbolTable;

/// <summary>
/// 単一クラスをVMコードに変換する
/// </summary>
public class Compiler
{
    Class _node;
    ClassSymbolTable _symbolTable = new ClassSymbolTable();
    StreamWriter _writer;

    string _uniqueLabelPrefix;  

    public Compiler(Class node, string outPath)
    {
        _node = node;
        _writer = new StreamWriter(outPath);
        _uniqueLabelPrefix = Path.GetFileNameWithoutExtension(outPath);
    }

    public void Compile()
    {
        _writer.Write(CompileClass(_node));
        _writer.Close();
    }

    // 'class' className '{' classVarDec* subroutineDec* '}'
    private string CompileClass(Class node)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"// Compiled class: {node.ClassName}");

        // クラススコープ開始
        _symbolTable.Reset(Scope.CLASS);

        // classVarDec*
        foreach (var classVarDec in node.ClassVarDecs)
        {
            sb.AppendLine(CompileClassVarDec(node.ClassName ,classVarDec));
        }

        foreach (var subroutineDec in node.SubroutineDecs)
        {
            sb.AppendLine(CompileSubroutineDec(node.ClassName, subroutineDec));
        }
        
        return sb.ToString().TrimEnd();
    }

    // ('static' | 'field') type varName (',' varName)* ';'
    private string CompileClassVarDec(string className, ClassVarDec node)
    {
        var sb = new StringBuilder();

        for (int i = 0; i < node.VarNames.Count; i++)
        {
            sb.AppendLine($"// Compiled class variable declaration: {node.Kwd.ToString().ToLower()} {className}.{node.VarNames[i]} of type {node.Type}");
            _symbolTable.Define(Scope.CLASS, node.VarNames[i], node.Type, node.Kwd == ScopeKwd.STATIC ? Segment.STATIC : Segment.THIS);
        }

        return sb.ToString().TrimEnd();
    }

    // ('constructor' | 'function' | 'method') ('void' | type) subroutineName '(' parameterList ')' subroutineBody
    private string CompileSubroutineDec(string className, SubroutineDec node)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"// Compiled subroutine declaration: {node.Kwd.ToString().ToLower()} {className}.{node.SubroutineName} with return type {(node.ReturnType == "void" ? "void" : node.ReturnType)}");

        // サブルーチンスコープ開始
        _symbolTable.Reset(Scope.SUBROUTINE);

        if (node.Kwd == SubroutineKwd.METHOD)
        {
            // !.1 method の argument 0 は暗黙の this
            _symbolTable.Define(Scope.SUBROUTINE, "this", className, Segment.ARGUMENT);
        }

        // パラメータをシンボルテーブルに登録
        foreach (var param in node.Params.Params)
        {
            sb.AppendLine($"// Compiled parameter: {className}.{node.SubroutineName} parameter '{param.Name}' of type {param.Type}");
            _symbolTable.Define(Scope.SUBROUTINE, param.Name, param.Type, Segment.ARGUMENT);
        }

        // ! パースして木を作ってあるので、ローカル変数の個数を先に処理できる
        int localVarCount = 0;
        foreach (var varDec in node.Body.VarDecs)
        {
            localVarCount += varDec.VarNames.Count;
        }

        // 関数呼び出しストラップ
        sb.AppendLine($"function {className}.{node.SubroutineName} {localVarCount}");
        if (node.Kwd == SubroutineKwd.CONSTRUCTOR)
        {   
            int fieldCount = _symbolTable.VarCount(Scope.CLASS);  // クラススコープのfieldの数を取得
            sb.AppendLine($"push constant {fieldCount}");
            sb.AppendLine("call Memory.alloc 1");
            sb.AppendLine("pop pointer 0");
        }
        else if (node.Kwd == SubroutineKwd.METHOD)
        {
            sb.AppendLine("push argument 0");
            sb.AppendLine("pop pointer 0");
        }

        sb.AppendLine(CompileSubroutineBody(className, node.SubroutineName, node.Body));

        // サブルーチンスコープ終了
        _symbolTable.Reset(Scope.SUBROUTINE);

        return sb.ToString().TrimEnd();
    }

    // '{' varDec* statements '}'
    private string CompileSubroutineBody(string className, string subroutineName, SubroutineBody node)
    {
        var sb = new StringBuilder();

        // varDec*
        foreach (var varDec in node.VarDecs)
        {
            sb.AppendLine(CompileVarDec(className, subroutineName, varDec));
        }

        sb.AppendLine(CompileStatements(className, subroutineName, node.Stmts));

        return sb.ToString().TrimEnd();
    }

    // 'var' type varName (',' varName)* ';'
    private string CompileVarDec(string className, string subroutineName, VarDec node)
    {
        var sb = new StringBuilder();

        for (int i = 0; i < node.VarNames.Count; i++)
        {
            sb.AppendLine($"// Compiled local variable declaration: {className}.{subroutineName} local variable '{node.VarNames[i]}' of type {node.Type}");
            _symbolTable.Define(Scope.SUBROUTINE, node.VarNames[i], node.Type, Segment.LOCAL);
        }

        return sb.ToString().TrimEnd();
    }

    // statements: statement*
    private string CompileStatements(string className, string subroutineName, PrintableList<Statement> statements)
    {
        var sb = new StringBuilder();

        foreach (var statement in statements)
        {
            switch (statement.Kwd)
            {
                case StatementKwd.LET:
                    sb.AppendLine(CompileLet(className, subroutineName, (LetStatement)statement.Body)); break;
                case StatementKwd.IF:
                    sb.AppendLine(CompileIf(className, subroutineName, (IfStatement)statement.Body)); break;
                case StatementKwd.WHILE:
                    sb.AppendLine(CompileWhile(className, subroutineName, (WhileStatement)statement.Body)); break;
                case StatementKwd.DO:
                    sb.AppendLine(CompileDo(className, subroutineName, (DoStatement)statement.Body)); break;
                case StatementKwd.RETURN:
                    sb.AppendLine(CompileReturn(className, subroutineName, (ReturnStatement)statement.Body)); break;
            }
        }

        return sb.ToString().TrimEnd();
    }

    // let varName ('[' expression ']')? '=' expression ';'
    // ! letはサブルーチン内でしか使えず、4つのセグメントいずれにもアクセスしうる
    private string CompileLet(string className, string subroutineName, LetStatement node)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"// Compiled let statement: {className}.{subroutineName} let {node.VarName} = (expression)");

        // * 右辺の値を計算
        sb.AppendLine(CompileExpression(className, subroutineName, node.ValueExpr));

        sb.AppendLine($"// Compiled let statement: {className}.{subroutineName} assign to variable '{node.VarName}'");

        // * 左辺に代入(pop)
        if(node.IndexExpr is not null)
        {
            // 1. 配列オフセットを計算
            sb.AppendLine(CompileExpression(className, subroutineName, node.IndexExpr));

            // 2. ベースアドレスをpush
            var (found, type, segment, index) = _symbolTable.Get(node.VarName);
            if (!found) throw new SemanticException($"Undefined variable: {node.VarName}");

            sb.AppendLine($"push {segment.ToString().ToLower()} {index}");
            sb.AppendLine("add");

            // 3. that にポインタをセット
            sb.AppendLine("pop pointer 1");

            // 4. スタックトップの値を that で参照されるアドレスに格納
            sb.AppendLine("pop that 0");
        }
        else
        {
            var (found, type, segment, index) = _symbolTable.Get(node.VarName);
            if (!found) throw new SemanticException($"Undefined variable: {node.VarName}");

            // 1. 代入先の決定
            if (segment == Segment.THIS)
            {
                sb.AppendLine($"pop this {index}");
            }
            else // static, argument, local
            {
                sb.AppendLine($"pop {segment.ToString().ToLower()} {index}");
            }            
        }

        return sb.ToString().TrimEnd();
    }

    static int globalIfCount = 0;
    public string CompileIf(string className, string subroutineName, IfStatement node)
    {
        StringBuilder sb = new StringBuilder();

        globalIfCount++;
        int ifCount = globalIfCount;  // ネストしたif文でもラベルが衝突しないように、グローバルカウンタから番号をもらう

        sb.AppendLine($"// Compiled if statement: {className}.{subroutineName} if (expression) {{ ... }} else {{ ... }}");

        sb.AppendLine($"// Compiled if condition:");
        sb.AppendLine(CompileExpression(className, subroutineName, node.Condition));
        sb.AppendLine($"if-goto IF_TRUE_{_uniqueLabelPrefix}.{ifCount}");
        sb.AppendLine($"goto IF_FALSE_{_uniqueLabelPrefix}.{ifCount}");

        sb.AppendLine($"// Compiled if true branch:");
        sb.AppendLine($"label IF_TRUE_{_uniqueLabelPrefix}.{ifCount}");
        sb.AppendLine(CompileStatements(className, subroutineName, node.ThenStmts));

        if (node.ElseStmts is not null && node.ElseStmts.Count > 0)
        {
            sb.AppendLine($"// Compiled else branch:");
            sb.AppendLine($"goto IF_END_{_uniqueLabelPrefix}.{ifCount}");
            sb.AppendLine($"label IF_FALSE_{_uniqueLabelPrefix}.{ifCount}");
            sb.AppendLine(CompileStatements(className, subroutineName, node.ElseStmts));
            sb.AppendLine($"label IF_END_{_uniqueLabelPrefix}.{ifCount}");
        }
        else
        {
            sb.AppendLine($"label IF_FALSE_{_uniqueLabelPrefix}.{ifCount}");
        }

        return sb.ToString().TrimEnd();
    }

    static int globalWhileCount = 0;
    public string CompileWhile(string className, string subroutineName, WhileStatement node)
    {
        StringBuilder sb = new StringBuilder();

        globalWhileCount++;
        int whileCount = globalWhileCount;  // ネストしたwhile文でもラベルが衝突しないように、グローバルカウンタから番号をもらう

        sb.AppendLine($"// Compiled while statement: {className}.{subroutineName} while (expression) {{ ... }}");
        sb.AppendLine($"label WHILE_COND_{_uniqueLabelPrefix}.{whileCount}");
        sb.AppendLine(CompileExpression(className, subroutineName, node.Condition));
        sb.AppendLine($"if-goto WHILE_BODY_{_uniqueLabelPrefix}.{whileCount}");
        sb.AppendLine($"goto WHILE_END_{_uniqueLabelPrefix}.{whileCount}");

        sb.AppendLine($"// Compiled while body:");
        sb.AppendLine($"label WHILE_BODY_{_uniqueLabelPrefix}.{whileCount}");
        sb.AppendLine(CompileStatements(className, subroutineName, node.BodyStmts));
        sb.AppendLine($"goto WHILE_COND_{_uniqueLabelPrefix}.{whileCount}");
        sb.AppendLine($"label WHILE_END_{_uniqueLabelPrefix}.{whileCount}");

        return sb.ToString().TrimEnd();
    }

    public string CompileDo(string className, string subroutineName, DoStatement node)
    {
            StringBuilder sb = new StringBuilder();
    
            sb.AppendLine($"// Compiled do statement: {className}.{subroutineName} do subroutineCall;");
            sb.AppendLine(CompileSubroutineCall(className, subroutineName, node.CallTerm));
    
            // do文は返り値を無視するので、スタックトップの値を捨てる
            sb.AppendLine("pop temp 0");
    
            return sb.ToString().TrimEnd();

    }

    public string CompileReturn(string className, string subroutineName, ReturnStatement node)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"// Compiled return statement: {className}.{subroutineName} return (expression);");

        if (node.ReturnExpr is not null)
        {
            sb.AppendLine(CompileExpression(className, subroutineName, node.ReturnExpr));
        }
        else
        {
            // voidの場合は return 0
            sb.AppendLine("push constant 0");
        }

        sb.AppendLine("return");

        return sb.ToString().TrimEnd();
    }

    private string CompileExpression(string className, string subroutineName, Expression expr)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine(CompileTerm(className, subroutineName, expr.FirstTerm));

        for (int i = 0; i < expr.RestTerms.Count; i++)
        {
            var op = expr.RestTerms[i].Op;
            var term = expr.RestTerms[i].Term;

            sb.AppendLine(CompileTerm(className, subroutineName, term));

            switch (op)
            {
                case Operator.ADD: sb.AppendLine("add"); break;
                case Operator.SUB: sb.AppendLine("sub"); break;
                case Operator.MUL: sb.AppendLine("call Math.multiply 2"); break;
                case Operator.DIV: sb.AppendLine("call Math.divide 2"); break;
                case Operator.AND: sb.AppendLine("and"); break;
                case Operator.OR: sb.AppendLine("or"); break;
                case Operator.LT: sb.AppendLine("lt"); break;
                case Operator.GT: sb.AppendLine("gt"); break;
                case Operator.EQ: sb.AppendLine("eq"); break;
            }
        }

        return sb.ToString().TrimEnd();
    }

    private string CompileTerm(string className, string subroutineName, Term term)
    {
        StringBuilder sb = new StringBuilder();

        switch (term)
        {
            case IntegerConstant ic:
                sb.AppendLine($"push constant {ic.Value}"); break;
            case StringConstant sc:
                sb.AppendLine($"push constant {sc.Value.Length}");
                sb.AppendLine("call String.new 1");
                foreach (char c in sc.Value)
                {
                    // TODO: char - int 対応の確認
                    sb.AppendLine($"push constant {(int)c}");
                    sb.AppendLine("call String.appendChar 2");
                }
                break;
            case KeywordConstant kc:
                if (kc.Value == ConstKwd.TRUE)
                {
                    sb.AppendLine("push constant 0");
                    sb.AppendLine("not");
                }
                else if (kc.Value == ConstKwd.FALSE || kc.Value == ConstKwd.NULL)
                {
                    sb.AppendLine("push constant 0");
                }
                else if (kc.Value == ConstKwd.THIS)
                {
                    sb.AppendLine("push pointer 0");
                }
                break;
            case VarNameTerm vn:
                var (found, type, segment, index) = _symbolTable.Get(vn.VarName);
                if (!found) throw new SemanticException($"Undefined variable: {vn.VarName}");

                sb.AppendLine($"push {segment.ToString().ToLower()} {index}");
                break;
            case VarNameIndexTerm vni:
                var (found2, type2, segment2, index2) = _symbolTable.Get(vni.VarName);
                if (!found2) throw new SemanticException($"Undefined variable: {vni.VarName}");

                sb.AppendLine(CompileExpression(className, subroutineName, vni.IndexExpr));
                sb.AppendLine($"push {segment2.ToString().ToLower()} {index2}");
                sb.AppendLine("add");
                sb.AppendLine("pop pointer 1");
                sb.AppendLine("push that 0");
                break;
            case ParenthesizedTerm parenTerm:
                sb.AppendLine(CompileExpression(className, subroutineName, parenTerm.InnerExpr));
                break;
            case UnaryOpTerm unaryOpTerm:
                sb.AppendLine(CompileTerm(className, subroutineName, unaryOpTerm.InnerTerm));
                if (unaryOpTerm.Op == UnaryOperator.NEG)
                {
                    sb.AppendLine("neg");
                }
                else if (unaryOpTerm.Op == UnaryOperator.NOT)
                {
                    sb.AppendLine("not");
                }
                break;
            case SubroutineCallTerm callTerm:
                sb.AppendLine(CompileSubroutineCall(className, subroutineName, callTerm));
                break;
            default:
                throw new SemanticException($"Unknown term type: {term.GetType()}");
        }

        return sb.ToString().TrimEnd();
    }


    private string CompileSubroutineCall(string className, string subroutineName, SubroutineCallTerm callTerm)
    {
        StringBuilder sb = new StringBuilder();

        string callClassName;
        string callSubroutineName;

        int argCount = callTerm.Arguments.Count;

        if (callTerm.ClassOrVarName is null)
        {
            // サブルーチン名のみの場合は、同クラスのサブルーチンとみなす
            callClassName = className;
            callSubroutineName = callTerm.SubroutineName;

            // メソッド呼び出しになるので、オブジェクト自身を引数に追加
            sb.AppendLine("push pointer 0");
            argCount++;
        }
        else
        {
            // クラス名 or 変数名 + サブルーチン名
            var (found, type, segment, index) = _symbolTable.Get(callTerm.ClassOrVarName);
            if (found)
            {
                // 変数名だった場合は、その変数の型がクラス名となる
                callClassName = type;
                callSubroutineName = callTerm.SubroutineName;

                // !.1 method の argument 0 である暗黙の this を追加
                sb.AppendLine($"push {segment.ToString().ToLower()} {index}");
                argCount++;
            }
            else
            {
                // ! クラス名だった場合は、そのままクラス名を使う
                callClassName = callTerm.ClassOrVarName;
                callSubroutineName = callTerm.SubroutineName;
            }
        }

        foreach (var arg in callTerm.Arguments)
        {
            sb.AppendLine(CompileExpression(className, subroutineName, arg));
        }

        sb.AppendLine($"call {callClassName}.{callSubroutineName} {argCount}");

        return sb.ToString().TrimEnd();
    }
}


/// <summary>
/// 識別子のスコープを管理するクラス
/// static + fieldはクラススコープ、arg + varはサブルーチンスコープ
/// </summary>
public class ClassSymbolTable
{
    Dictionary<string, (string type, Segment segment, int index)> _localTable = new();
    Dictionary<string, (string type, Segment segment, int index)> _classTable = new();

    public enum Scope
    {
        CLASS,
        SUBROUTINE
    }

    public enum Segment
    {
        STATIC,
        THIS,
        ARGUMENT,
        LOCAL
    }

    public void Reset(Scope scope)
    {
        if (scope == Scope.CLASS)
        {
            _classTable.Clear();
        }
        else
        {
            _localTable.Clear();
        }
    }

    /// <summary>
    /// 識別子をスコープに登録する
    /// </summary>
    /// <param name="scope">CLASS か SUBROUTINE</param>
    /// <param name="name">変数名</param>
    /// <param name="type">変数のスコープでの種類</param>
    /// <param name="segment">変数のセグメント</param>
    public void Define(Scope scope, string name, string type, Segment segment)
    {
        if (scope == Scope.CLASS)
        {
            int index = _classTable.Values.Count(v => v.segment == segment);
            _classTable[name] = (type, segment, index);
        }
        else
        {
            int index = _localTable.Values.Count(v => v.segment == segment);
            _localTable[name] = (type, segment, index);
        }
    }

    public int VarCount(Scope scope)
    {
        if (scope == Scope.CLASS)
        {
            return _classTable.Count;
        }
        else
        {
            return _localTable.Count;
        }
    }

    public (bool found, string type, Segment segment, int index) Get(string name)
    {
        if (_localTable.ContainsKey(name))
        {
            var (type, segment, index) = _localTable[name];
            return (true, type, segment, index);
        }
        else if (_classTable.ContainsKey(name))
        {
            var (type, segment, index) = _classTable[name];
            return (true, type, segment, index);
        }
        else
        {
            return (false, "", Segment.LOCAL, -1);
        }
    }
}

public class SemanticException : Exception
{
    public SemanticException(string message) : base(message) { }
}
