using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Divide_Operator : Operator {

    public override float evaluate(float input)
    {
        return Child1.evaluate(input) / Child2.evaluate(input);
    }

    public override string asString()
    {
        string b = Child1.asString();
        string c = Child2.asString();
        string s = "/ " + b + " " + c;
        return s;
    }

    public override Node differentiate()
    {
        Minus_Operator a = new Minus_Operator();
        Multiply_Operator m1 = new Multiply_Operator();
        Multiply_Operator m2 = new Multiply_Operator();
        Node c1 = Child1.differentiate();
        Node c2 = Child2;
        Node c3 = Child1;
        Node c4 = Child2.differentiate();
        Divide_Operator d = new Divide_Operator();
        Exponent_Operator e = new Exponent_Operator();
        Node c5 = Child2;
        Operand o = new Operand(2);

        a.Child1 = m1;
        a.Child2 = m2;
        m1.Parent = a;
        m2.Parent = a;

        m1.Child1 = c1;
        c1.Parent = m1;
        m1.Child2 = c2;
        c2.Parent = m1;

        m2.Child1 = c3;
        m2.Child2 = c4;
        c3.Parent = m2;
        c4.Parent = m2;

        e.Child1 = c5;
        e.Child2 = o;
        o.Parent = e;
        c5.Parent = e;

        d.Child1 = a;
        d.Child2 = e;
        a.Parent = d;
        e.Parent = e;

        return d;
    }

    public override Node simplify()
    {
        throw new System.NotImplementedException();
    }
    public override float Solve(float n)
    {
        return 0.0f;
    }
}
