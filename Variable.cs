using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variable : Node
{
    public override string asString()
    {
        return " x ";
    }

    public override float evaluate(float input)
    {
        return input;
    }

    public override Node differentiate()
    {

        return new Operand(1);
    }

    public override float Solve(float n)
    {
        return n;
    }

    public override Node simplify()
    {
        throw new System.NotImplementedException();
    }
}
