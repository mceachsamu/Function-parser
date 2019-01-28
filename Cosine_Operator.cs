using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cosine_Operator : Operator
{
    public override string asString()
    {
        return "cos(" + Child1.asString() + ")";
    }

    public override float evaluate(float input)
    {
        return Mathf.Cos(Child1.evaluate(input));
    }

    public override Node differentiate()
    {
        Node p = new Sin_Operator();
        Node c1 = Child1;
        Multiply_Operator m = new Multiply_Operator();
        Node c2 = c1.differentiate();
        m.Child1 = c2;
        p.Child1 = c1;
        m.Child2 = p;
        p.Parent = m;
        c2.Parent = m;

        Multiply_Operator m2 = new Multiply_Operator();
        Operand o = new Operand(-1);
        m2.Child1 = o;
        m2.Child2 = m;
        m.Parent = m2;
        o.Parent = m2;

        return m2;
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
