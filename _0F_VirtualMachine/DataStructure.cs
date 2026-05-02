enum CommandType
{
    C_ARITHMETIC,
    C_PUSH,
    C_POP,

    C_LABEL,
    C_GOTO,
    C_IF,
    C_FUNCTION,
    C_RETURN,
    C_CALL
}

struct Command
{
    public CommandType commandType;
    public string arg1;
    public int arg2;
}
