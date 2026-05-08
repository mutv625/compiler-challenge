
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
    
    (_f_INIT_LOOP$_Sys.Sys.init)
        // end loop if D <= 0
            @_f_INIT_END$_Sys.Sys.init
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
            @_f_INIT_LOOP$_Sys.Sys.init
            0;JMP

    (_f_INIT_END$_Sys.Sys.init)
        
//// # call Main.main 0
    // push @RET_ADDR of this call
        @_RET_ADDR$Main.main_9
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
    
    // goto function Main.main
        @_f$Main.main
        0;JMP

    // function returns here = @RET_ADDR
    (_RET_ADDR$Main.main_9)
        
//// label END
    (_L$END)
        
//// goto END
    @_L$END
        0;JMP
        
//// # function Main.main 1 <REPEAT VER>
    (_f$Main.main)
    
    // push 0 * 1 times; D = (counter)
        @1
        D=A
    
    (_f_INIT_LOOP$Main.Main.main)
        // end loop if D <= 0
            @_f_INIT_END$Main.Main.main
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
            @_f_INIT_LOOP$Main.Main.main
            0;JMP

    (_f_INIT_END$Main.Main.main)
        
//// # push constant value
    // D = @value
        @8001
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
        @16
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
        @1
        D=A
    
    // M[M[SP]] = D (value saved)
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

    // D = - D
        D=-D

    // push D
        @SP
        A=M

        M=D

        // SP++
        @SP
        M=M+1
        
//// # call Main.fillMemory 3
    // push @RET_ADDR of this call
        @_RET_ADDR$Main.fillMemory_1
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

        @3
        D=D-A

        @ARG
        M=D

    // new LCL = SP
        @SP
        D=M

        @LCL
        M=D
    
    // goto function Main.fillMemory
        @_f$Main.fillMemory
        0;JMP

    // function returns here = @RET_ADDR
    (_RET_ADDR$Main.fillMemory_1)
        
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
        @8000
        D=A
    
    // M[M[SP]] = D (value saved)
        @SP
        A=M
        M=D

    // SP++
        @SP
        M=M+1
                
//// # call Memory.peek 1
    // push @RET_ADDR of this call
        @_RET_ADDR$Memory.peek_2
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
    
    // goto function Memory.peek
        @_f$Memory.peek
        0;JMP

    // function returns here = @RET_ADDR
    (_RET_ADDR$Memory.peek_2)
        
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
                
//// # call Main.convert 1
    // push @RET_ADDR of this call
        @_RET_ADDR$Main.convert_3
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
    
    // goto function Main.convert
        @_f$Main.convert
        0;JMP

    // function returns here = @RET_ADDR
    (_RET_ADDR$Main.convert_3)
        
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
        
//// # function Main.convert 3 <REPEAT VER>
    (_f$Main.convert)
    
    // push 0 * 3 times; D = (counter)
        @3
        D=A
    
    (_f_INIT_LOOP$Main.Main.convert)
        // end loop if D <= 0
            @_f_INIT_END$Main.Main.convert
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
            @_f_INIT_LOOP$Main.Main.convert
            0;JMP

    (_f_INIT_END$Main.Main.convert)
        
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
        @2
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
                
//// label WHILE_COND_1
    (_L$WHILE_COND_1)
        
//// # push SEGMENT index
    // A = M[@SEGMENT] + @index
        @LCL
        A=M
        D=A
        @2
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
                
//// if-goto WHILE_BODY_1
    // pop cond to D
        @SP
        M=M-1
        A=M

        D=M

    // jump to @_L$WHILE_BODY_1 if D != 0
        @_L$WHILE_BODY_1
        D;JNE

        
//// goto WHILE_END_1
    @_L$WHILE_END_1
        0;JMP
        
//// label WHILE_BODY_1
    (_L$WHILE_BODY_1)
        
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
        @1
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
                
//// # call Main.nextMask 1
    // push @RET_ADDR of this call
        @_RET_ADDR$Main.nextMask_4
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
    
    // goto function Main.nextMask
        @_f$Main.nextMask
        0;JMP

    // function returns here = @RET_ADDR
    (_RET_ADDR$Main.nextMask_4)
        
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
                
//// # push constant value
    // D = @value
        @16
        D=A
    
    // M[M[SP]] = D (value saved)
        @SP
        A=M
        M=D

    // SP++
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
        D=D-M
    
    // jump to @PUSH_TRUE if (JGT)
        @_PUSH_TRUE$Main.1
        D;JGT
    // jump to @PUSH_FALSE else
        @_PUSH_FALSE$Main.1
        0;JMP

    (_PUSH_TRUE$Main.1)
    // push True == 0xFFFF by !@0
        @0
        A=!A
        D=A

        @SP
        A=M

        M=D

        // SP++
        @SP
        M=M+1

        @_COMPR_END$Main.1
        0;JMP

    (_PUSH_FALSE$Main.1)
    // push False == 0x0000
        @0
        D=A

        @SP
        A=M

        M=D

        // SP++
        @SP
        M=M+1

        @_COMPR_END$Main.1
        0;JMP

        (_COMPR_END$Main.1)
        
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
        
//// if-goto IF_TRUE_1
    // pop cond to D
        @SP
        M=M-1
        A=M

        D=M

    // jump to @_L$IF_TRUE_1 if D != 0
        @_L$IF_TRUE_1
        D;JNE

        
//// goto IF_FALSE_1
    @_L$IF_FALSE_1
        0;JMP
        
//// label IF_TRUE_1
    (_L$IF_TRUE_1)
        
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

    // D = D(x) & M[R13](y)
        @R13
        D=D&M

    // push D
        @SP
        A=M

        M=D

        // SP++
        @SP
        M=M+1
        
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
        D=D-M
    
    // jump to @PUSH_TRUE if (JEQ)
        @_PUSH_TRUE$Main.2
        D;JEQ
    // jump to @PUSH_FALSE else
        @_PUSH_FALSE$Main.2
        0;JMP

    (_PUSH_TRUE$Main.2)
    // push True == 0xFFFF by !@0
        @0
        A=!A
        D=A

        @SP
        A=M

        M=D

        // SP++
        @SP
        M=M+1

        @_COMPR_END$Main.2
        0;JMP

    (_PUSH_FALSE$Main.2)
    // push False == 0x0000
        @0
        D=A

        @SP
        A=M

        M=D

        // SP++
        @SP
        M=M+1

        @_COMPR_END$Main.2
        0;JMP

        (_COMPR_END$Main.2)
        
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
        
//// if-goto IF_TRUE_2
    // pop cond to D
        @SP
        M=M-1
        A=M

        D=M

    // jump to @_L$IF_TRUE_2 if D != 0
        @_L$IF_TRUE_2
        D;JNE

        
//// goto IF_FALSE_2
    @_L$IF_FALSE_2
        0;JMP
        
//// label IF_TRUE_2
    (_L$IF_TRUE_2)
        
//// # push constant value
    // D = @value
        @8000
        D=A
    
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
                
//// # call Memory.poke 2
    // push @RET_ADDR of this call
        @_RET_ADDR$Memory.poke_5
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
    
    // goto function Memory.poke
        @_f$Memory.poke
        0;JMP

    // function returns here = @RET_ADDR
    (_RET_ADDR$Memory.poke_5)
        
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
                
//// goto IF_END_2
    @_L$IF_END_2
        0;JMP
        
//// label IF_FALSE_2
    (_L$IF_FALSE_2)
        
//// # push constant value
    // D = @value
        @8000
        D=A
    
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
                
//// # call Memory.poke 2
    // push @RET_ADDR of this call
        @_RET_ADDR$Memory.poke_6
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
    
    // goto function Memory.poke
        @_f$Memory.poke
        0;JMP

    // function returns here = @RET_ADDR
    (_RET_ADDR$Memory.poke_6)
        
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
                
//// label IF_END_2
    (_L$IF_END_2)
        
//// goto IF_END_1
    @_L$IF_END_1
        0;JMP
        
//// label IF_FALSE_1
    (_L$IF_FALSE_1)
        
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
        @2
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
                
//// label IF_END_1
    (_L$IF_END_1)
        
//// goto WHILE_COND_1
    @_L$WHILE_COND_1
        0;JMP
        
//// label WHILE_END_1
    (_L$WHILE_END_1)
        
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
        
//// # function Main.nextMask 0 <REPEAT VER>
    (_f$Main.nextMask)
    
    // push 0 * 0 times; D = (counter)
        @0
        D=A
    
    (_f_INIT_LOOP$Main.Main.nextMask)
        // end loop if D <= 0
            @_f_INIT_END$Main.Main.nextMask
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
            @_f_INIT_LOOP$Main.Main.nextMask
            0;JMP

    (_f_INIT_END$Main.Main.nextMask)
        
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
        @0
        D=A
    
    // M[M[SP]] = D (value saved)
        @SP
        A=M
        M=D

    // SP++
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
        D=D-M
    
    // jump to @PUSH_TRUE if (JEQ)
        @_PUSH_TRUE$Main.3
        D;JEQ
    // jump to @PUSH_FALSE else
        @_PUSH_FALSE$Main.3
        0;JMP

    (_PUSH_TRUE$Main.3)
    // push True == 0xFFFF by !@0
        @0
        A=!A
        D=A

        @SP
        A=M

        M=D

        // SP++
        @SP
        M=M+1

        @_COMPR_END$Main.3
        0;JMP

    (_PUSH_FALSE$Main.3)
    // push False == 0x0000
        @0
        D=A

        @SP
        A=M

        M=D

        // SP++
        @SP
        M=M+1

        @_COMPR_END$Main.3
        0;JMP

        (_COMPR_END$Main.3)
        
//// if-goto IF_TRUE_3
    // pop cond to D
        @SP
        M=M-1
        A=M

        D=M

    // jump to @_L$IF_TRUE_3 if D != 0
        @_L$IF_TRUE_3
        D;JNE

        
//// goto IF_FALSE_3
    @_L$IF_FALSE_3
        0;JMP
        
//// label IF_TRUE_3
    (_L$IF_TRUE_3)
        
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
        
//// goto IF_END_3
    @_L$IF_END_3
        0;JMP
        
//// label IF_FALSE_3
    (_L$IF_FALSE_3)
        
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
        @2
        D=A
    
    // M[M[SP]] = D (value saved)
        @SP
        A=M
        M=D

    // SP++
        @SP
        M=M+1
                
//// # call Math.multiply 2
    // push @RET_ADDR of this call
        @_RET_ADDR$Math.multiply_7
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
    
    // goto function Math.multiply
        @_f$Math.multiply
        0;JMP

    // function returns here = @RET_ADDR
    (_RET_ADDR$Math.multiply_7)
        
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
        
//// label IF_END_3
    (_L$IF_END_3)
        
//// # function Main.fillMemory 0 <REPEAT VER>
    (_f$Main.fillMemory)
    
    // push 0 * 0 times; D = (counter)
        @0
        D=A
    
    (_f_INIT_LOOP$Main.Main.fillMemory)
        // end loop if D <= 0
            @_f_INIT_END$Main.Main.fillMemory
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
            @_f_INIT_LOOP$Main.Main.fillMemory
            0;JMP

    (_f_INIT_END$Main.Main.fillMemory)
        
//// label WHILE_COND_2
    (_L$WHILE_COND_2)
        
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
        D=D-M
    
    // jump to @PUSH_TRUE if (JGT)
        @_PUSH_TRUE$Main.4
        D;JGT
    // jump to @PUSH_FALSE else
        @_PUSH_FALSE$Main.4
        0;JMP

    (_PUSH_TRUE$Main.4)
    // push True == 0xFFFF by !@0
        @0
        A=!A
        D=A

        @SP
        A=M

        M=D

        // SP++
        @SP
        M=M+1

        @_COMPR_END$Main.4
        0;JMP

    (_PUSH_FALSE$Main.4)
    // push False == 0x0000
        @0
        D=A

        @SP
        A=M

        M=D

        // SP++
        @SP
        M=M+1

        @_COMPR_END$Main.4
        0;JMP

        (_COMPR_END$Main.4)
        
//// if-goto WHILE_BODY_2
    // pop cond to D
        @SP
        M=M-1
        A=M

        D=M

    // jump to @_L$WHILE_BODY_2 if D != 0
        @_L$WHILE_BODY_2
        D;JNE

        
//// goto WHILE_END_2
    @_L$WHILE_END_2
        0;JMP
        
//// label WHILE_BODY_2
    (_L$WHILE_BODY_2)
        
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
        @ARG
        A=M
        D=A
        @2
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
                
//// # call Memory.poke 2
    // push @RET_ADDR of this call
        @_RET_ADDR$Memory.poke_8
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
    
    // goto function Memory.poke
        @_f$Memory.poke
        0;JMP

    // function returns here = @RET_ADDR
    (_RET_ADDR$Memory.poke_8)
        
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
        @1
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
                
//// goto WHILE_COND_2
    @_L$WHILE_COND_2
        0;JMP
        
//// label WHILE_END_2
    (_L$WHILE_END_2)
        
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
        