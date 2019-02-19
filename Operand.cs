using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Operand : Node
{

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
        return "" + Value;
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

    public override void Mutate(int range, float probability)
    {
        if (probability > Random.Range(0.0f, 1.0f))
        {
            float decide = Random.Range(0.0f, 3.0f);
            if (this.Parent.GetType() != typeof(Exponent_Operator))//set to a new value operand
            {
                //Debug.Log("changing operand to new value");
                this.Value = Random.Range(-range, range);
            }
            /*else if (decide <= 2)//set to variable operand
            {
                //Debug.Log("changing operand to variable");
                if (this.Parent != null)
                {
                    if (this.Parent.Child1 == this)
                    {
                        this.Parent.Child1 = new Variable();
                        this.Parent.Child1.Parent = this.Parent;
                    }
                    else if (this.Parent.Child2 == this)
                    {
                        this.Parent.Child2 = new Variable();
                        this.Parent.Child2.Parent = this.Parent;
                    }
                    else
                    {
                        Debug.Assert(false);
                    }
                }
            } else if (decide <= 3)//set to random operator with basic value operand children
            {

               // Debug.Log("changing operand to operator " + this.asString());
                Operator op = new Add_Operator();
                float decideOperator = Random.Range(0, 7);
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
                try
                {
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
                    else
                    {
                        Debug.Assert(false);
                    }
                } catch (System.NullReferenceException e)
                {
                    Debug.Log("error " + this.getHead().asString());
                    throw new System.NullReferenceException();
                }
                //want to only add one child if its a sin or cosine operator
                if(op is Cosine_Operator || op is Sin_Operator)
                {
                    op.Child1 = new Operand(1);
                    op.Child1.Parent = op;
                } else
                {
                    op.Child1 = new Operand(1);
                    op.Child2 = new Operand(1);
                    op.Child1.Parent = op;
                    op.Child2.Parent = op;
                }
            }*/
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