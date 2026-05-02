
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
        @6
        D=A
    
    // M[M[SP]] = D (value saved)
        @SP
        A=M
        M=D

    // SP++
        @SP
        M=M+1
                
//// # push constant value
    // D = @value
        @8
        D=A
    
    // M[M[SP]] = D (value saved)
        @SP
        A=M
        M=D

    // SP++
        @SP
        M=M+1
                
//// # call Class1.set 2
    // push @RET_ADDR of this call
        @_RET_ADDR$Class1.set_1
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

        @2
        D=D-A

        @ARG
        M=D

    // new LCL = SP
        @SP
        D=M

        @LCL
        M=D
    
    // goto Class1.set
        @_f$Class1.set
        0;JMP

    // function returns here = @RET_ADDR
    (_RET_ADDR$Class1.set_1)
        
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

    // M[R14] = (the address to save the popped value)
        @5
        D=A

        @R14
        M=D
    
    // D = M[R13] (popped value)
        @R13
        D=M

    // M[M[R14]] = D
        @R14
        A=M
        M=D
                
//// # push constant value
    // D = @value
        @23
        D=A
    
    // M[M[SP]] = D (value saved)
        @SP
        A=M
        M=D

    // SP++
        @SP
        M=M+1
                
//// # push constant value
    // D = @value
        @15
        D=A
    
    // M[M[SP]] = D (value saved)
        @SP
        A=M
        M=D

    // SP++
        @SP
        M=M+1
                
//// # call Class2.set 2
    // push @RET_ADDR of this call
        @_RET_ADDR$Class2.set_2
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

        @2
        D=D-A

        @ARG
        M=D

    // new LCL = SP
        @SP
        D=M

        @LCL
        M=D
    
    // goto Class2.set
        @_f$Class2.set
        0;JMP

    // function returns here = @RET_ADDR
    (_RET_ADDR$Class2.set_2)
        
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

    // M[R14] = (the address to save the popped value)
        @5
        D=A

        @R14
        M=D
    
    // D = M[R13] (popped value)
        @R13
        D=M

    // M[M[R14]] = D
        @R14
        A=M
        M=D
                
//// # call Class1.get 0
    // push @RET_ADDR of this call
        @_RET_ADDR$Class1.get_3
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

        @0
        D=D-A

        @ARG
        M=D

    // new LCL = SP
        @SP
        D=M

        @LCL
        M=D
    
    // goto Class1.get
        @_f$Class1.get
        0;JMP

    // function returns here = @RET_ADDR
    (_RET_ADDR$Class1.get_3)
        
//// # call Class2.get 0
    // push @RET_ADDR of this call
        @_RET_ADDR$Class2.get_4
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

        @0
        D=D-A

        @ARG
        M=D

    // new LCL = SP
        @SP
        D=M

        @LCL
        M=D
    
    // goto Class2.get
        @_f$Class2.get
        0;JMP

    // function returns here = @RET_ADDR
    (_RET_ADDR$Class2.get_4)
        
//// label END
    (_L$END)
        
//// goto END
    @_L$END
        0;JMP
        