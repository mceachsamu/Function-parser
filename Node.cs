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
