using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Add_Operator : Operator
{

    public override float evaluate(float input)
    {
        return Child1.evaluate(input) + Child2.evaluate(input);
    }

    public override string asString()
    {
        string b = Child1.asString();
        string c = Child2.asString();
        string s = "+ " + b + " " + c;
        return s;
    }

    public override Node differentiate()
    {
        Node n = this;
        Node c1 = Child1.differentiate();
        Node c2 = Child2.differentiate();
        n.Child1 = c1;
        n.Child2 = c2;
        c1.Parent = n;
        c2.Parent = n;
        return this;
    }

    public override float Solve(float n)
    {
        float Left = Child1.Solve(n);
        float Right = Child2.Solve(n - Left);
        if (Child1.evaluate(Left) + Child2.evaluate(Left) != n)
        {
            if (Child1.evaluate(Right) + Child2.evaluate(Right) != n)
            {
                Right = Child2.Solve(n);
                Left = Child1.Solve(n - Right);
                if (Child1.evaluate(Left) + Child2.evaluate(Left) != n)
                {
                    if (Child1.evaluate(Right) + Child2.evaluate(Right) != n)
                    {
                        return float.PositiveInfinity;
                    }
                    return Right;
                }
                return Left;
            }
            return Right;
        }
        return Left;
    }

    public override Node simplify()
    {
        throw new System.NotImplementedException();
    }

}
