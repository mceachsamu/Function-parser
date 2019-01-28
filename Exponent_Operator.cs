using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exponent_Operator : Operator {

    public override float evaluate(float input)
    {
        return Mathf.Pow(Child1.evaluate(input), Child2.evaluate(input));
    }

    public override string asString()
    {
        string b = Child1.asString();
        string c = Child2.asString();
        return "(" + b + "^" + c + ")";
    }

    public override Node differentiate()
    {
        Minus_Operator m = new Minus_Operator();
        Operand o = new Operand(1);
        Node c = Child2;
        Multiply_Operator mult = new Multiply_Operator();
        mult.Child1 = Child1;
        mult.Child2 =  c.differentiate();
        mult.Parent = this;
        this.Child1 = mult;
        m.Child2 = o;
        m.Child1 = Child2;
        Child2 = m;
        return this;
    }


    public override float Solve(float n)
    {
        return 0.0f;
    }

    public override Node simplify()
    {
        throw new System.NotImplementedException();
    }
}
