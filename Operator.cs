using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Operator : Node {

    public override void Mutate(int range, float probability)
    {
        Debug.Assert(probability >= 0 && probability <= 1);

        if(false)//change operator to another operator or an operand
        {
            //Debug.Log("from this view: " + this.Parent.GetType() + " " + this.GetType() + " " + this.Child1.GetType() + " " + this.Child2.GetType());
           // Debug.Log("from parent: " + this.Parent.Child1.GetType());
            //Debug.Log("from child view: " + this.Child2.Parent.GetType() + " " + this.Child2.Parent.GetType());
            float decide = Random.Range(0.0f, 8.0f);
            if(decide < 0)
            {
                //Debug.Log("changing operator to add");
                Node newNode = new Add_Operator();
                this.replaceNode(newNode);
            }else if(decide < 1)
            {
                //Debug.Log("changing operator to minus");
                Node newNode = new Minus_Operator();
                this.replaceNode(newNode);
            }else if(decide < 2)
            {
                //Debug.Log("changing operator to mult");
                Node newNode = new Multiply_Operator();
                this.replaceNode(newNode);
            }else if(decide < 3)
            {
                //Debug.Log("changing operator to divide");
                Node newNode = new Divide_Operator();
                this.replaceNode(newNode);
            }else if(decide < 4)
            {
                //Debug.Log("changing operator to cos");
                Node newNode = new Cosine_Operator();
                this.replaceNode(newNode);
            }else if(decide < 5)
            {
                //Debug.Log("changing operator to sin");
                Node newNode = new Sin_Operator();
                this.replaceNode(newNode);
            }else if(decide < 6)
            {
                //Debug.Log("changing operator to exponent");
                Node newNode = new Exponent_Operator();
                this.replaceNode(newNode);
            }else if(decide < 7)
            {
                //Debug.Log("changing operator to operand");
                Node newNode = new Operand(Random.Range(-range, range));
                this.replaceNode(newNode);
            }else if(decide < 8)
            {
                //Debug.Log("changing operator to variable");
                Node newNode = new Variable();
                this.replaceNode(newNode);
            }
        }
        if (Child1 != null)
        {
            Child1.Mutate(range, probability);
        }
        if(Child2 != null)
        {
            Child2.Mutate(range, probability);
        }
    }

}
