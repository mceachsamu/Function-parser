using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sin_Operator : Operator
{
    public override string asString()
    {
        return "sin(" + Child1.asString() + ")";
    }

    public override float evaluate(float input)
    {
        return Mathf.Sin(Child1.evaluate(input));
    }

    public override Node differentiate()
    {
        Node p = new Cosine_Operator();
        Node c1 = Child1;
        Multiply_Operator m = new Multiply_Operator();
        Node c2 = c1.differentiate();
        m.Child1 = c2;
        p.Child1 = c1;
        m.Child2 = p;
        p.Parent = m;
        c2.Parent = m;
        return m;
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
