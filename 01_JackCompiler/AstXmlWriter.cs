public class AstXmlWriter
{
    string _outPath;
    StreamWriter _writer;

    Class _node;

    public AstXmlWriter(Class node, string outPath)
    {
        _node = node;
        _outPath = outPath;
        _writer = new StreamWriter(_outPath);
    }

    public void Write()
    {
        _writer.Write(WriteClass(_node));
        _writer.Close();
    }

    // 'class' className '{' classVarDec* subroutineDec* '}'
    private string WriteClass(Class node)
    {
        var xmlBuilder = new XmlStringBuilder();

        xmlBuilder.AppendLine("<class>");
        xmlBuilder.AppendLine("<keyword> class </keyword>");
        xmlBuilder.AppendLine($"<identifier> {node.ClassName} </identifier>");

        xmlBuilder.AppendLine("<symbol> { </symbol>");

        // * classVarDec
        foreach (var classVarDec in node.ClassVarDecs)
        {
            xmlBuilder.AppendLine(WriteClassVarDec(classVarDec));
        }

        // * subroutineDec
        foreach (var subroutineDec in node.SubroutineDecs)
        {
            xmlBuilder.AppendLine(WriteSubroutineDec(subroutineDec));
        }

        xmlBuilder.AppendLine("<symbol> } </symbol>");
        xmlBuilder.AppendLine("</class>");
        return xmlBuilder.ToString();
    }

    // ('static' | 'field') type varName (',' varName)* ';'
    private string WriteClassVarDec(ClassVarDec node)
    {
        var xmlBuilder = new XmlStringBuilder();

        xmlBuilder.AppendLine("<classVarDec>");
        xmlBuilder.AppendLine($"<keyword> {node.Kwd.ToString().ToLower()} </keyword>");
        xmlBuilder.AppendLine($"<keyword> {node.Type} </keyword>");

        for (int i = 0; i < node.VarNames.Count; i++)
        {
            xmlBuilder.AppendLine($"<identifier> {node.VarNames[i]} </identifier>");

            if (i < node.VarNames.Count - 1)
            {
                xmlBuilder.AppendLine($"<symbol> , </symbol>");
            }
        }

        xmlBuilder.AppendLine("<symbol> ; </symbol>");
        xmlBuilder.AppendLine("</classVarDec>");

        return xmlBuilder.ToString();
    }

    // ('constructor' | 'function' | 'method') ('void' | type) subroutineName '(' parameterList ')' subroutineBody
    private string WriteSubroutineDec(SubroutineDec node)
    {
        var xmlBuilder = new XmlStringBuilder();

        xmlBuilder.AppendLine("<subroutineDec>");
        xmlBuilder.AppendLine($"<keyword> {node.Kwd.ToString().ToLower()} </keyword>");
        xmlBuilder.AppendLine($"<keyword> {node.ReturnType} </keyword>");

        xmlBuilder.AppendLine($"<identifier> {node.SubroutineName} </identifier>");

        // * parameterList
        xmlBuilder.AppendLine("<symbol> ( </symbol>");
        xmlBuilder.AppendLine(WriteParameterList(node.Parameters));
        xmlBuilder.AppendLine("<symbol> ) </symbol>");

        // * subroutineBody
        xmlBuilder.AppendLine(WriteSubroutineBody(node.Body));

        xmlBuilder.AppendLine("</subroutineDec>");

        return xmlBuilder.ToString();
    }

    // ((type varName) (',' type varName)*)?
    private string WriteParameterList(ParameterList node)
    {
        var xmlBuilder = new XmlStringBuilder();

        xmlBuilder.AppendLine("<parameterList>");

        for (int i = 0; i < node.Parameters.Count; i++)
        {
            var parameter = node.Parameters[i];
            xmlBuilder.AppendLine($"<keyword> {parameter.Type} </keyword>");
            xmlBuilder.AppendLine($"<identifier> {parameter.Name} </identifier>");

            if (i < node.Parameters.Count - 1)
            {
                xmlBuilder.AppendLine("<symbol> , </symbol>");
            }
        }

        xmlBuilder.AppendLine("</parameterList>");

        return xmlBuilder.ToString();
    }

    // subroutineBody: '{' varDec* statements '}'
    private string WriteSubroutineBody(SubroutineBody node)
    {
        var xmlBuilder = new XmlStringBuilder();

        xmlBuilder.AppendLine("<subroutineBody>");
        xmlBuilder.AppendLine("<symbol> { </symbol>");

        // * varDec
        foreach (var varDec in node.VarDecs)
        {
            xmlBuilder.AppendLine(WriteVarDec(varDec));
        }

        // * statements
        xmlBuilder.AppendLine(WriteStatements(node.Statements));

        xmlBuilder.AppendLine("<symbol> } </symbol>");
        xmlBuilder.AppendLine("</subroutineBody>");

        return xmlBuilder.ToString();
    }

    // 'var' type varName (',' varName)* ';'
    private string WriteVarDec(VarDec node)
    {
        var xmlBuilder = new XmlStringBuilder();

        xmlBuilder.AppendLine("<varDec>");
        xmlBuilder.AppendLine("<keyword> var </keyword>");
        xmlBuilder.AppendLine($"<keyword> {node.Type} </keyword>");

        for (int i = 0; i < node.VarNames.Count; i++)
        {
            xmlBuilder.AppendLine($"<identifier> {node.VarNames[i]} </identifier>");

            if (i < node.VarNames.Count - 1)
            {
                xmlBuilder.AppendLine("<symbol> , </symbol>");
            }
        }

        xmlBuilder.AppendLine("<symbol> ; </symbol>");
        xmlBuilder.AppendLine("</varDec>");

        return xmlBuilder.ToString();
    }

    // statement*
    private string WriteStatements(PrintableList<Statement> statements)
    {
        var xmlBuilder = new XmlStringBuilder();

        xmlBuilder.AppendLine("<statements>");

        foreach (var statement in statements)
        {
            switch (statement.Kwd)
            {
                case StatementKwd.LET:
                    xmlBuilder.AppendLine(WriteLetStatement((LetStatement)statement.Body)); break;
                case StatementKwd.IF:
                    xmlBuilder.AppendLine(WriteIfStatement((IfStatement)statement.Body)); break;
                case StatementKwd.WHILE:
                    xmlBuilder.AppendLine(WriteWhileStatement((WhileStatement)statement.Body)); break;
                case StatementKwd.DO:
                    xmlBuilder.AppendLine(WriteDoStatement((DoStatement)statement.Body)); break;
                case StatementKwd.RETURN:
                    xmlBuilder.AppendLine(WriteReturnStatement((ReturnStatement)statement.Body)); break;
            }
        }

        xmlBuilder.AppendLine("</statements>");

        return xmlBuilder.ToString();
    }

    // 'let' varName ('[' expression ']')? '=' expression ';'
    private string WriteLetStatement(LetStatement node)
    {
        var xmlBuilder = new XmlStringBuilder();

        xmlBuilder.AppendLine("<letStatement>");
        xmlBuilder.AppendLine("<keyword> let </keyword>");
        xmlBuilder.AppendLine($"<identifier> {node.VarName} </identifier>");

        if (node.IndexExpression != null)
        {
            xmlBuilder.AppendLine("<symbol> [ </symbol>");
            xmlBuilder.AppendLine(WriteExpression(node.IndexExpression));
            xmlBuilder.AppendLine("<symbol> ] </symbol>");
        }

        xmlBuilder.AppendLine("<symbol> = </symbol>");

        xmlBuilder.AppendLine(WriteExpression(node.ValueExpression));

        xmlBuilder.AppendLine("<symbol> ; </symbol>");
        xmlBuilder.AppendLine("</letStatement>");

        return xmlBuilder.ToString();
    }

    // 'if' '(' expression ')' '{' statements '}' ('else' '{' statements '}')?
    private string WriteIfStatement(IfStatement node)
    {
        var xmlBuilder = new XmlStringBuilder();

        xmlBuilder.AppendLine("<ifStatement>");
        xmlBuilder.AppendLine("<keyword> if </keyword>");
        xmlBuilder.AppendLine("<symbol> ( </symbol>");
        xmlBuilder.AppendLine(WriteExpression(node.Condition));
        xmlBuilder.AppendLine("<symbol> ) </symbol>");

        xmlBuilder.AppendLine("<symbol> { </symbol>");
        xmlBuilder.AppendLine(WriteStatements(node.ThenStatements));
        xmlBuilder.AppendLine("<symbol> } </symbol>");

        if (node.ElseStatements != null)
        {
            xmlBuilder.AppendLine("<keyword> else </keyword>");
            xmlBuilder.AppendLine("<symbol> { </symbol>");
            xmlBuilder.AppendLine(WriteStatements(node.ElseStatements));
            xmlBuilder.AppendLine("<symbol> } </symbol>");
        }

        xmlBuilder.AppendLine("</ifStatement>");

        return xmlBuilder.ToString();
    }

    // 'while' '(' expression ')' '{' statements '}'
    private string WriteWhileStatement(WhileStatement node)
    {
        var xmlBuilder = new XmlStringBuilder();

        xmlBuilder.AppendLine("<whileStatement>");
        xmlBuilder.AppendLine("<keyword> while </keyword>");
        xmlBuilder.AppendLine("<symbol> ( </symbol>");
        xmlBuilder.AppendLine(WriteExpression(node.Condition));
        xmlBuilder.AppendLine("<symbol> ) </symbol>");

        xmlBuilder.AppendLine("<symbol> { </symbol>");
        xmlBuilder.AppendLine(WriteStatements(node.BodyStatements));
        xmlBuilder.AppendLine("<symbol> } </symbol>");
        xmlBuilder.AppendLine("</whileStatement>");

        return xmlBuilder.ToString();
    }

    // 'do' subroutineCall ';'
    private string WriteDoStatement(DoStatement node)
    {
        var xmlBuilder = new XmlStringBuilder();

        xmlBuilder.AppendLine("<doStatement>");
        xmlBuilder.AppendLine("<keyword> do </keyword>");
        xmlBuilder.AppendLine(WriteSubroutineCall(node.Call));
        xmlBuilder.AppendLine("<symbol> ; </symbol>");
        xmlBuilder.AppendLine("</doStatement>");

        return xmlBuilder.ToString();
    }

    // 'return' expression? ';'
    private string WriteReturnStatement(ReturnStatement node)
    {
        var xmlBuilder = new XmlStringBuilder();

        xmlBuilder.AppendLine("<returnStatement>");
        xmlBuilder.AppendLine("<keyword> return </keyword>");

        if (node.ReturnExpr != null)
        {
            xmlBuilder.AppendLine(WriteExpression(node.ReturnExpr));
        }

        xmlBuilder.AppendLine("<symbol> ; </symbol>");
        xmlBuilder.AppendLine("</returnStatement>");

        return xmlBuilder.ToString();
    }

    // term (op term)*
    private string WriteExpression(Expression node)
    {
        var xmlBuilder = new XmlStringBuilder();

        xmlBuilder.AppendLine("<expression>");

        xmlBuilder.AppendLine(WriteTerm(node.FirstTerm));

        foreach (var (Op, Term) in node.RestTerms)
        {
            xmlBuilder.AppendLine($"<symbol> {Op.Escape()} </symbol>");
            xmlBuilder.AppendLine(WriteTerm(Term));
        }

        xmlBuilder.AppendLine("</expression>");

        return xmlBuilder.ToString();
    }

    private string WriteTerm(Term node)
    {
        var xmlBuilder = new XmlStringBuilder();

        xmlBuilder.AppendLine("<term>");

        switch (node)
        {
            case IntegerConstant intConst:
                xmlBuilder.AppendLine($"<integerConstant> {intConst.Value} </integerConstant>");
                break;
            case StringConstant strConst:
                xmlBuilder.AppendLine($"<stringConstant> {strConst.Value} </stringConstant>");
                break;
            case KeywordConstant kwdConst:
                xmlBuilder.AppendLine($"<keyword> {kwdConst.Value.ToString().ToLower()} </keyword>");
                break;
            case VarNameTerm varName:
                xmlBuilder.AppendLine($"<identifier> {varName.VarName} </identifier>");
                break;
            case VarNameIndexTerm varNameIndex:
                xmlBuilder.AppendLine($"<identifier> {varNameIndex.VarName} </identifier>");
                xmlBuilder.AppendLine("<symbol> [ </symbol>");
                xmlBuilder.AppendLine(WriteExpression(varNameIndex.IndexExpression));
                xmlBuilder.AppendLine("<symbol> ] </symbol>");
                break;
            case ParenthesizedTerm parenTerm:
                xmlBuilder.AppendLine("<symbol> ( </symbol>");
                xmlBuilder.AppendLine(WriteExpression(parenTerm.InnerExpression));
                xmlBuilder.AppendLine("<symbol> ) </symbol>");
                break;
            case SubroutineCallTerm subroutineCall:
                xmlBuilder.AppendLine(WriteSubroutineCall(subroutineCall));
                break;
            case UnaryOpTerm unaryOpTerm:
                xmlBuilder.AppendLine($"<symbol> {unaryOpTerm.Op.Escape()} </symbol>");
                xmlBuilder.AppendLine(WriteTerm(unaryOpTerm.InnerTerm));
                break;
        }

        xmlBuilder.AppendLine("</term>");

        return xmlBuilder.ToString();
    }

    // subroutineName '(' expressionList ')' | (className | varName) '.' subroutineName '(' expressionList ')'
    private string WriteSubroutineCall(SubroutineCallTerm node)
    {
        var xmlBuilder = new XmlStringBuilder();

        // (className | varName) '.'
        if (node.ClassOrVarName != null)
        {
            xmlBuilder.AppendLine($"<identifier> {node.ClassOrVarName} </identifier>");
            xmlBuilder.AppendLine("<symbol> . </symbol>");
        }

        xmlBuilder.AppendLine($"<identifier> {node.SubroutineName} </identifier>");
        xmlBuilder.AppendLine("<symbol> ( </symbol>");
        xmlBuilder.AppendLine("<expressionList>");
        for (int i = 0; i < node.Arguments.Count; i++)
        {
            xmlBuilder.AppendLine(WriteExpression(node.Arguments[i]));

            if (i < node.Arguments.Count - 1)
            {
                xmlBuilder.AppendLine("<symbol> , </symbol>");
            }
        }
        xmlBuilder.AppendLine("</expressionList>");
        xmlBuilder.AppendLine("<symbol> ) </symbol>");

        return xmlBuilder.ToString();
    }
}


public static class XmlEscaper
{
    public static string Escape(this Operator op)
    {
        return op switch
        {
            Operator.PLUS => "+",
            Operator.MINUS => "-",
            Operator.ASTERISK => "*",
            Operator.SLASH => "/",
            Operator.AMPERSAND => "&amp;",
            Operator.PIPE => "|",
            Operator.LT => "&lt;",
            Operator.GT => "&gt;",
            Operator.EQ => "=",
            _ => throw new ArgumentOutOfRangeException(nameof(op), $"Unexpected operator: {op}")
        };
    }

    public static string Escape(this UnaryOperator op)
    {
        return op switch
        {
            UnaryOperator.MINUS => "-",
            UnaryOperator.TILDE => "~",
            _ => throw new ArgumentOutOfRangeException(nameof(op), $"Unexpected unary operator: {op}")
        };
    }
}

public class XmlStringBuilder
{
    private string _result = "";
    int _indentLevel = 0;

    public void AppendLine(string line)
    {
        _result += new string('\t', _indentLevel) + line + "\n";
    }

    public void Indent()
    {
        _indentLevel++;
    }

    public void Unindent()
    {
        if (_indentLevel > 0)
            _indentLevel--;
    }


    public override string ToString()
    {
        return _result.TrimEnd();
    }
}

