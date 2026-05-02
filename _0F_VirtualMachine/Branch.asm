//// # label {symbol}
(_L${symbol})

//// # goto {symbol}
@_L${symbol}
0;JMP

//// # if-goto {symbol}
    // pop cond to D
        @SP
        M=M-1
        A=M

        D=M

    // conditional JMP
    // jump to @_L${symbol} if D != 0
        @_L${symbol}
        D;JNE


//// # call {func} {nArgs}
    // push @RET_ADDR
        @_RET_ADDR${fileName}.{func}
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

        @{nArgs}
        D=D-A

        @ARG
        M=D

    // new LCL = SP
        @SP
        D=M

        @LCL
        M=Data
    
    // goto {func}
        @_f${fileName}.{func}
        0;JMP

    // function returns here = @RET_ADDR
    (_RET_ADDR${fileName}.{func})


//// # function {func} {nVars}
    (_f${fileName}.{func})
    
    // push 0 * {nVars} times
        @SP
        A=M
    
    // * repeat writing {nVars} times
            M=0
            A=A+1

    // finally SP = (last A)
        D=A

        @SP
        M=D

//// # function {func} {nVars} <REPEAT VER>
    (_f${fileName}.{func})
    
    // push 0 * {nVars} times; D = (counter)
        @{nVars}
        D=A
    
    (_f_INIT_LOOP${fileName}.{func})
        // end loop if D<=0
            @_f_INIT_END${fileName}.{func}
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
            @_f_INIT_LOOP${fileName}.{func}
            0;JMP

    (_f_INIT_END${fileName}.{func})


//// # return

    // R13(frameTop) = LCL
        @LCL
        D=M

        @R13
        M=D
    
    // R14(returnAddr) = M[D(frameTop) - 5]
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
        // D = A(@ARG) + 1
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
