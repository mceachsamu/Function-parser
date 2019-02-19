using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variable : Node
{

   
    public override string asString()
    {
        return "x";
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

    public override void Mutate(int range, float probability)
    {
        if (false)
        {
            float decide = Random.Range(0.0f, 3.0f);
            if (decide <= 1)//set to a new value operand
            {
                //Debug.Log("changing variable to value operand");
                if(this.Parent.Child1 == this)
                {
                    Operand o = new Operand(Random.Range(-range, range));
                    o.Parent = this.Parent;
                    this.Parent.Child1 = o;
                }
                else if(this.Parent.Child2 == this)
                {
                    Operand o = new Operand(Random.Range(-range, range));
                    o.Parent = this.Parent;
                    this.Parent.Child2 = o;
                }
                else
                {
                    Debug.Assert(false);
                }
            }
            else if (decide <= 2)//set to variable
            {
                //Debug.Log("variable is staying as variable");
                
            }
            else if (decide <= 3)//set to random operator with basic value operand children
            {
               // Debug.Log("changing variable to operator");
                Operator op = new Add_Operator();
                float decideOperator = Random.Range(0.0f, 7.0f);
                //generator randome operator
                if (decideOperator <= 1)
                {
                    op = new Add_Operator();
                }
                else if (decideOperator <= 2)
                {
                    op = new Minus_Operator();
                }
                else if (decideOperator <= 3)
                {
                    op = new Multiply_Operator();
                }
                else if (decideOperator <= 4)
                {
                    op = new Divide_Operator();
                }
                else if (decideOperator <= 5)
                {
                    op = new Exponent_Operator();
                }
                else if (decideOperator <= 6)
                {
                    op = new Sin_Operator();
                }
                else if (decideOperator <= 7)
                {
                    op = new Cosine_Operator();
                }
                //find which of the parents children is this object
                if (this.Parent.Child1 == this)
                {
                    this.Parent.Child1 = op;
                    op.Parent = this.Parent;
                }
                else if (this.Parent.Child2 == this)
                {
                    this.Parent.Child2 = op;
                    op.Parent = this.Parent;
                }
                //want to only add one child if its a sin or cosine operator
                if (op is Cosine_Operator || op is Sin_Operator)
                {
                    op.Child1 = new Operand(1);
                    op.Child1.Parent = op;
                }
                else
                {
                    op.Child1 = new Operand(1);
                    op.Child2 = new Operand(1);
                    op.Child1.Parent = op;
                    op.Child2.Parent = op;
                }
            }
        }
        if (Child1 != null)
        {
            Child1.Mutate(range, probability);
        }
        if (Child2 != null)
        {
            Child2.Mutate(range, probability);
        }

    }
}
