//// # push SEGMENT index
    // A = M[@SEGMENT] + @index
        @{SEGMENT}
        A=M
        D=A
        @index
        A=D+A

    // D = M[A] (now D have value to push)
        D=M

    // M[M[SP]] = D (value saved)
        @SP
        A=M
        M=D

    // M[SP]++
        @SP
        M=M+1


//// # <push/pop> pointer <0/1>
// treat as SEGMENT == <THIS/THAT> , index = 0

//// # <push/pop> temp index
// accessing to RAM[5+i] (0<=i<=7)
// treat as SEGMENT == R5, index = i

//// # push constant value
    // D = @value
        @{value}
        D=A
    
    // M[M[SP]] = D (value saved)
        @SP
        A=M
        M=D

    // M[SP]++
        @SP
        M=M+1



//// # pop SEGMENT index
    // M[SP]--
        @SP
        M=M-1
    
    // D = M[ M[A(=SP)] ] (now D have a popped value)
        A=M
        D=M
    
    // M[R13] = D (pass the popped value from D to R13)
        @R13
        M=D

    // M[R14] = M[@SEGMENT] + @index (record the address to save the popped value)
        @{SEGMENT}
        A=M
        D=A
        @{index}
        D=D+A

        @R14
        M=D
    
    // D = M[R13] (popped value)
        @R13
        D=M

    // M[M[R14]] = D
        @R14
        A=M
        M=D




//// # add, sub, and, or
    // pop y to M[R13]
        @SP
        M=M-1
        A=M

        D=M

        @R13
        M=D

    // pop x to D
        @SP
        M=M-1
        A=M

        D=M

    // D = D(x) {OPCODE} M[R13](y)
        @R13
        D=D+M

    // push D
        @SP
        A=M

        M=D

        // M[SP]++
        @SP
        M=M+1


//// # neg, not
    // pop to D
        @SP
        M=M-1
        A=M

        D=M

    // D = {OPCODE} D
        D=-D

    // push D
        @SP
        A=M

        M=D

        // M[SP]++
        @SP
        M=M+1
        

////  # eq, lt, gt
    // pop y to M[R13]
        @SP
        M=M-1
        A=M

        D=M

        @R13
        M=D

    // pop x to D
        @SP
        M=M-1
        A=M

        D=M

    // D = D(x) - M[R13](y)
        @R13
        D=D+M
    
    // jump to @$PUSH_TRUE if (D <eq/gt/lt>)
        @_FileName$PUSH_TRUE_1
        D;JEQ
    // jump to @$PUSH_FALSE else
        @_FileName$PUSH_FALSE_1
        0;JMP

    (_FileName$PUSH_TRUE_1)
    // push 0xFFFF
        @65535
        D=A

        @SP
        A=M

        M=D

        // M[SP]++
        @SP
        M=M+1

        @_FileName$END_1
        0;JMP

    (_FileName$PUSH_FALSE_1)
    // push 0
        @0
        D=A

        @SP
        A=M

        M=D

        // M[SP]++
        @SP
        M=M+1

        @_FileName$END_1
        0;JMP

    (_FileName$END_1)