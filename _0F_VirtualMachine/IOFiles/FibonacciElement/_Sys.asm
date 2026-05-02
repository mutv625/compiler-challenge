
//// # function Sys.init 0 <REPEAT VER>
    (_f$Sys.init)
    
    // push 0 * 0 times; D = (counter)
        @0
        D=A
    
    (_f_INIT_LOOP$Sys.Sys.init)
        // end loop if D <= 0
            @_f_INIT_END$Sys.Sys.init
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
            @_f_INIT_LOOP$Sys.Sys.init
            0;JMP

    (_f_INIT_END$Sys.Sys.init)
        
//// # push constant value
    // D = @value
        @4
        D=A
    
    // M[M[SP]] = D (value saved)
        @SP
        A=M
        M=D

    // SP++
        @SP
        M=M+1
                
//// # call Main.fibonacci 1
    // push @RET_ADDR of this call
        @_RET_ADDR$Main.fibonacci_3
        D=A

        @SP
        A=M

        M=D

        // M[SP]++
        @SP
        M=M+1
    
    // push LCL = M[@LCL]
        @LCL
        D=M

        @SP
        A=M
        M=D

        // M[SP]++
        @SP
        M=M+1

    // push ARG = M[@ARG]
        @ARG
        D=M

        @SP
        A=M
        M=D

        // M[SP]++
        @SP
        M=M+1

    // push THIS = M[@THIS]
        @THIS
        D=M

        @SP
        A=M
        M=D

        // M[SP]++
        @SP
        M=M+1

    // push THAT = M[@THAT]
        @THAT
        D=M

        @SP
        A=M
        M=D

        // M[SP]++
        @SP
        M=M+1

    // new ARG = SP - 5 - nArgs
        @SP
        D=M

        @5
        D=D-A

        @1
        D=D-A

        @ARG
        M=D

    // new LCL = SP
        @SP
        D=M

        @LCL
        M=D
    
    // goto Main.fibonacci
        @_f$Main.fibonacci
        0;JMP

    // function returns here = @RET_ADDR
    (_RET_ADDR$Main.fibonacci_3)
        
//// label END
    (_L$END)
        
//// goto END
    @_L$END
        0;JMP
        