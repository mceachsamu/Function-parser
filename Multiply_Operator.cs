using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiply_Operator : Operator
{

    public override float evaluate(float input)
    {
        return Child1.evaluate(input) * Child2.evaluate(input);
    }

    public override string asString()
    {
        string b = Child1.asString();
        string c = Child2.asString();
        return "(" + b + "*" + c + ")";
    }

    public override Node differentiate()
    {
        Add_Operator a1 = new Add_Operator();
        Multiply_Operator m1 = new Multiply_Operator();
        Multiply_Operator m2 = new Multiply_Operator();
        Node c1 = Child1;
        Node c2 = Child2.differentiate();
        Node c3 = Child1.differentiate();
        Node c4 = Child2;
        a1.Child1 = m1;
        m1.Parent = a1;
        a1.Child2 = m2;
        m2.Parent = a1;

        m1.Child1 = c1;
        c1.Parent = m1;
        m1.Child2 = c2;
        c2.Parent = m1;

        m2.Child1 = c3;
        c3.Parent = m2;
        m2.Child2 = c4;
        c4.Parent = m2;

        return a1;
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
