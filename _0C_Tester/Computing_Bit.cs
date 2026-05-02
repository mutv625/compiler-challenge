using System;

using static BasicGates.Bit.BooleanLogic;
using static BasicGates.Bit.SwitchingGate;

namespace Computing.Bit;

public class BooleanCalc
{
    public static (bool car, bool s) HalfAdder(bool a, bool b)
    {
        bool car = AND(a, b);
        bool s = XOR(a, b);
        return (car, s);
    }

    public static (bool car, bool s) FullAdder(bool a, bool b, bool cin)
    {
        (bool car1, bool s1) = HalfAdder(a, b);
        (bool car2, bool s) = HalfAdder(s1, cin);
        bool car = OR(car1, car2);
        return (car, s);
    }
}