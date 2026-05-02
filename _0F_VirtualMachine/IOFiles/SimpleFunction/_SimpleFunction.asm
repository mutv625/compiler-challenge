
//// # function SimpleFunction.test 2 <REPEAT VER>
    (_f$SimpleFunction.SimpleFunction.test)
    
    // push 0 * 2 times; D = (counter)
        @2
        D=A
    
    (_f_INIT_LOOP$SimpleFunction.SimpleFunction.test)
        // end loop if D <= 0
            @_f_INIT_END$SimpleFunction.SimpleFunction.test
            D;JLE

        // M[SP] = 0
            @SP
            A=M

            M=0
        
        // SP++
            @SP
            M=M+1

        // D(counter)--
            D=D-1

        // back to begin
            @_f_INIT_LOOP$SimpleFunction.SimpleFunction.test
            0;JMP

    (_f_INIT_END$SimpleFunction.SimpleFunction.test)
        
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
                
//// # push SEGMENT index
    // A = M[@SEGMENT] + @index
        @LCL
        A=M
        D=A
        @1
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
        
//// # neg, not
    // pop to D
        @SP
        M=M-1
        A=M

        D=M

    // D = ! D
        D=!D

    // push D
        @SP
        A=M

        M=D

        // SP++
        @SP
        M=M+1
        
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
        
//// # push SEGMENT index
    // A = M[@SEGMENT] + @index
        @ARG
        A=M
        D=A
        @1
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
        
//// # return

    // R13(frameTop) = LCL
        @LCL
        D=M

        @R13
        M=D
    
    // R14(returnAddr) = M[D(framrTop) - 5]
        // A = D - 5
        @5
        A=D-A
        
        D=M

        @R14
        M=D

    // pop to M[ARG]
        @SP
        M=M-1
        A=M
        // now D have popped value
        D=M

        @ARG
        A=M
        M=D
    // SP = ARG + 1
        @ARG
        A=M
        A=A+1
        D=A
        
        @SP
        M=D


    // THAT = M[R13--]
        @R13
        M=M-1
        A=M

        D=M
        
        @THAT
        M=D

    // THIS = M[R13--]
        @R13
        M=M-1
        A=M

        D=M
        
        @THIS
        M=D

    // ARG = M[R13--]
        @R13
        M=M-1
        A=M

        D=M
        
        @ARG
        M=D
    
    // LCL = M[R13--]
        @R13
        M=M-1
        A=M

        D=M
        
        @LCL
        M=D

    // goto R14(returnAddr)
        @R14
        A=M
        0;JMP
        