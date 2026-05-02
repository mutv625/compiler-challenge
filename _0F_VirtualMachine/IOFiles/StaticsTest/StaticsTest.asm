
// # Bootstrap code
    @256
    D=A
    @SP
    M=D

//// # call Sys.init 0
    // push @RET_ADDR of this call
        @_RET_ADDR$Sys.init_1
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
    
    // goto Sys.init
        @_f$Sys.init
        0;JMP

    // function returns here = @RET_ADDR
    (_RET_ADDR$Sys.init_1)
  
        
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
        
//// # function Class1.set 0 <REPEAT VER>
    (_f$Class1.set)
    
    // push 0 * 0 times; D = (counter)
        @0
        D=A
    
    (_f_INIT_LOOP$Class1.Class1.set)
        // end loop if D <= 0
            @_f_INIT_END$Class1.Class1.set
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
            @_f_INIT_LOOP$Class1.Class1.set
            0;JMP

    (_f_INIT_END$Class1.Class1.set)
        
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
        @$Class1.0
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
        @$Class1.1
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
        @0
        D=A
    
    // M[M[SP]] = D (value saved)
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
        
//// # function Class1.get 0 <REPEAT VER>
    (_f$Class1.get)
    
    // push 0 * 0 times; D = (counter)
        @0
        D=A
    
    (_f_INIT_LOOP$Class1.Class1.get)
        // end loop if D <= 0
            @_f_INIT_END$Class1.Class1.get
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
            @_f_INIT_LOOP$Class1.Class1.get
            0;JMP

    (_f_INIT_END$Class1.Class1.get)
        
//// # push SEGMENT index
    // A = 5 + index
        @$Class1.0

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
    // A = 5 + index
        @$Class1.1

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
        
//// # function Class2.set 0 <REPEAT VER>
    (_f$Class2.set)
    
    // push 0 * 0 times; D = (counter)
        @0
        D=A
    
    (_f_INIT_LOOP$Class2.Class2.set)
        // end loop if D <= 0
            @_f_INIT_END$Class2.Class2.set
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
            @_f_INIT_LOOP$Class2.Class2.set
            0;JMP

    (_f_INIT_END$Class2.Class2.set)
        
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
        @$Class2.0
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
        @$Class2.1
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
        @0
        D=A
    
    // M[M[SP]] = D (value saved)
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
        
//// # function Class2.get 0 <REPEAT VER>
    (_f$Class2.get)
    
    // push 0 * 0 times; D = (counter)
        @0
        D=A
    
    (_f_INIT_LOOP$Class2.Class2.get)
        // end loop if D <= 0
            @_f_INIT_END$Class2.Class2.get
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
            @_f_INIT_LOOP$Class2.Class2.get
            0;JMP

    (_f_INIT_END$Class2.Class2.get)
        
//// # push SEGMENT index
    // A = 5 + index
        @$Class2.0

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
    // A = 5 + index
        @$Class2.1

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
        