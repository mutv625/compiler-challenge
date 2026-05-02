
//// # push constant value
    // D = @value
        @0
        D=A
    
    // M[M[SP]] = D (value saved)
        @SP
        A=M
        M=D

    // SP++
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
        @LCL
        A=M
        D=A
        @0
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
                
//// label LOOP
    (_L$LOOP)
        
//// # push SEGMENT index
    // A = M[@SEGMENT] + @index
        @ARG
        A=M
        D=A
        @0
        A=D+A

    // D = M[A] (now D have value to push)
        D=M

    // M[M[SP]] = D (value saved)
        @SP
        A=M
        M=D

    // SP++
        @SP
        M=M+1
                
//// # push SEGMENT index
    // A = M[@SEGMENT] + @index
        @LCL
        A=M
        D=A
        @0
        A=D+A

    // D = M[A] (now D have value to push)
        D=M

    // M[M[SP]] = D (value saved)
        @SP
        A=M
        M=D

    // SP++
        @SP
        M=M+1
                
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

    // D = D(x) + M[R13](y)
        @R13
        D=D+M

    // push D
        @SP
        A=M

        M=D

        // SP++
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
        @LCL
        A=M
        D=A
        @0
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
                
//// # push SEGMENT index
    // A = M[@SEGMENT] + @index
        @ARG
        A=M
        D=A
        @0
        A=D+A

    // D = M[A] (now D have value to push)
        D=M

    // M[M[SP]] = D (value saved)
        @SP
        A=M
        M=D

    // SP++
        @SP
        M=M+1
                
//// # push constant value
    // D = @value
        @1
        D=A
    
    // M[M[SP]] = D (value saved)
        @SP
        A=M
        M=D

    // SP++
        @SP
        M=M+1
                
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

    // D = D(x) - M[R13](y)
        @R13
        D=D-M

    // push D
        @SP
        A=M

        M=D

        // SP++
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
        @ARG
        A=M
        D=A
        @0
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
                
//// # push SEGMENT index
    // A = M[@SEGMENT] + @index
        @ARG
        A=M
        D=A
        @0
        A=D+A

    // D = M[A] (now D have value to push)
        D=M

    // M[M[SP]] = D (value saved)
        @SP
        A=M
        M=D

    // SP++
        @SP
        M=M+1
                
//// if-goto LOOP
    // pop cond to D
        @SP
        M=M-1
        A=M

        D=M

    // jump to @_L$LOOP if D != 0
        @_L$LOOP
        D;JNE

        
//// # push SEGMENT index
    // A = M[@SEGMENT] + @index
        @LCL
        A=M
        D=A
        @0
        A=D+A

    // D = M[A] (now D have value to push)
        D=M

    // M[M[SP]] = D (value saved)
        @SP
        A=M
        M=D

    // SP++
        @SP
        M=M+1
                