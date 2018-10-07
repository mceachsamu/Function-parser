using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Operand : Node {

    float Value = 0.0f;
    
    public Operand(float value)
    {
        Value = value;
    }

    public override float evaluate(float input)
    {
        return Value;
    }

    public void setValue(float v)
    {
        Value = v;
    }

    public override string asString()
    {
        string val = " " + Value + " ";
        return val;
    }

    public override Node differentiate()
    {
        return new Operand(0);
    }


    public override float Solve(float n)
    {
        return Value;
    }

    public override Node simplify()
    {
        throw new System.NotImplementedException();
    }

}
