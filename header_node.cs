using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class header_node : Node
{
    

    public header_node(Node c)
    {
        Child1 = c;
    }

    public override float evaluate(float input)
    {
        return Child1.evaluate(input);
    }


    public override string asString()
    {
        return Child1.asString();
    }

    public override Node differentiate()
    {
        return Child1.differentiate();
    }


    public override float Solve(float n)
    {
        return 0.0f;
    }

    public override Node simplify()
    {
        throw new System.NotImplementedException();
    }

    public override void Mutate(int range, float probability)
    {
        Child1.Mutate(range, probability);
    }
}