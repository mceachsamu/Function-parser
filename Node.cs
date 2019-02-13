using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Node{
    public Node Child1 = null;
    public Node Child2 = null;
    public Node Parent = null;
    public void setChild1(Node n)
    {
        Child1 = n;
    }

    public void setChild2(Node n)
    {
        Child2 = n;
    }

    public void setParent(Node n)
    {
        Parent = n;
    }

    public Node getHead()
    {
        if(Parent == null)
        {
            return this;
        }
        return Parent.getHead();
    }
    
    public abstract Node differentiate();

    public abstract string asString();

    public abstract float evaluate(float input);

    public abstract float Solve(float n);

    public abstract void Mutate(int range, float probability);
    //replaces this current node with another without changing the structure (unless the changed node cant have children)

    public void replaceNode(Node newNode)
    {
        if (newNode is Cosine_Operator || newNode is Sin_Operator)
        {
            Operand o = new Operand(1);
            newNode.Child1 = o;
            newNode.Child1.Parent = newNode;
        }else if (newNode is Operator)
        {
            if(this.Child1 == null)
            {
                this.Child1 = new Operand(1);
            }
            if(this.Child2 == null)
            {
                this.Child2 = new Operand(1);
            }
            this.Child1.Parent = newNode;
            this.Child2.Parent = newNode;
            newNode.Child1 = this.Child1;
            newNode.Child2 = this.Child2;
        }
        //change the current nodes parent to point to new node
        if(this.Parent == null)
        {
           // Debug.Log(this.GetType() + " " + this.Child1.GetType() + this.Child2.GetType());
        }
        try
        {
            if (this.Parent.Child1 == this)
            {
                this.Parent.Child1 = newNode;
                newNode.Parent = this.Parent;
            }
            else if (this.Parent.Child2 == this)
            {
                this.Parent.Child2 = newNode;
                newNode.Parent = this.Parent;
            }
            else
            {
                // Debug.Log("view from current: " + this.GetType() + " " + Child1.GetType() + " " + Child2.GetType());
                //Debug.Log("view from parent " + this.Parent.GetType() + " " + this.Parent.Child1.GetType());
                //Debug.Log("view from children " + this.Child1.Parent.GetType() + " " + this.Child2.Parent.GetType());
                Debug.Assert(false);
            }
        }
        catch (System.NullReferenceException n)
        {
            Debug.Log("error: " + this.getHead().asString());
            //throw new System.NullReferenceException();
        }
       // Debug.Assert(newNode.Parent.Child1 == newNode || newNode.Parent.Child2 == newNode);
        Debug.Assert(newNode.Parent == this.Parent);
        if (newNode.Child1 != null)
        {
            Debug.Assert(newNode.Child1.Parent == newNode);
            if(newNode.Child2 != null)
            {
                Debug.Assert(newNode.Child2.Parent == newNode);
            }
        }
    }

    public Node FindNormalForm(Node n)
    {
        Minus_Operator m = new Minus_Operator();
        Node root1 = this.getHead();
        Node root2 = n.getHead();
        root1.Parent = m;
        root2.Parent = m;
        m.Child1 = root1;
        m.Child2 = root2;
        float independant = this.getHead().evaluate(0);
        return m;
    }

    public abstract Node simplify();//want to return a simplified version of this Node
    
}
