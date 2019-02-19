using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parser : MonoBehaviour {
    //global count is a global variable the program uses to keep track of its position
    //on the function text
    public int GlobalCount = 0;
    public Node currentNode = null;
    // Use this for initialization
    //want to avoid using expressions that are undefined/infinite at certain points ei 1/x, tan(x)
    //these expressions will cause alot of issues when rendering models from these function
    void Start () {
        Debug.Assert(false);
        runGeneralTests();
	}

    public Parser()
    {
       // runGeneralTests();
    }

   //simple comparison script between two strings
    public bool compareString(string s1, string s2)
    {
        char[] li = s1.ToCharArray();
        char[] li2 = s2.ToCharArray();
        for(int i = 0; i < li.Length; i++)
        {
            if(li[i+1] != li2[i])
            {
                return false;
            }
        }
        return true;
    }

//runs a series of test on multiple functions
    public void runTests()
    {
        currentNode = null;
        GlobalCount = 0;
        //Test 1 - test basic addition:
        string[] Test1 = { "15", "+", "5" };
        Node Node1 = ParseNodeTree(Test1);
        Debug.Assert(Node1.evaluate(0) == 20);

        currentNode = null;
        GlobalCount = 0;
        //Test 2 - test basic subtraction
        string[] Test2 = { "5", "-", "5" };
        Node Node2 = ParseNodeTree(Test2);
        Debug.Assert(Node2.evaluate(0) == 0);
        currentNode = null;
        GlobalCount = 0;
        //Test 3 - test basic multiplicaton
        string[] Test3 = { "5", "*", "5" };
        Node Node3 = ParseNodeTree(Test3);
        Debug.Assert(Node3.evaluate(0) == 25);

        currentNode = null;
        GlobalCount = 0;

        //Test 4 - test basic multiplicaton
        string[] Test4 = { "5", "/", "25" };
        Node Node4 = ParseNodeTree(Test4);
        Debug.Assert(Node4.evaluate(0) == 0.2f);

        currentNode = null;
        GlobalCount = 0;
        //Test 5 - test basic multiplicaton
        string[] Test5 = { "5", "/", "25" };
        Node Node5 = ParseNodeTree(Test4);
        Debug.Assert(Node5.evaluate(0) == 0.2f);
        //////////////////////////OK LETS STOP MESSING AROUND WOOO

        currentNode = null;
        GlobalCount = 0;
        //Test 6 - test addition with variable
        string[] Test6 = { "5", "+", "x" };
        Node Node6 = ParseNodeTree(Test6);
        Debug.Assert(Node6.evaluate(20) == 25);
        //assuming variables work with everything other operator since they work with addition because im lazy

        currentNode = null;
        GlobalCount = 0;
        //Test 7 - multiplication to addition
        string[] Test7 = { "5", "*", "2", "+", "2" };
        Node Node7 = ParseNodeTree(Test7);
        Debug.Assert(Node7.evaluate(20) == 12);
        currentNode = null;
        GlobalCount = 0;

        //Test 8 - multiplication to addition
        string[] Test8 = { "5", "+", "2", "*", "2" };
        Node Node8 = ParseNodeTree(Test8);
        Debug.Assert(Node8.evaluate(20) == 9);

        currentNode = null;
        GlobalCount = 0;

        //Test 9 - Division to addition
        string[] Test9 = { "5", "/", "2", "-", "2" };
        Node Node9 = ParseNodeTree(Test9);
        Debug.Assert(Node9.evaluate(20) == 0.5f);

        currentNode = null;
        GlobalCount = 0;

        //Test 10 - multiplication with addition with variable
        string[] Test10 = { "x", "/", "2", "-", "2" };
        Node Node10 = ParseNodeTree(Test10);
        Debug.Assert(Node10.evaluate(20) == 8);
        /////OK LETS TURN IT UP A NOTCH

        currentNode = null;
        GlobalCount = 0;

        //Test 11 - test getbracketText
        string[] test11 = { "(", "5", "+", "5", ")" };
        GlobalCount++;
        string[] s = getBracketText(test11);
        for (int i = 0; i < s.Length; i++)
        {
            Debug.Assert(s[i] == test11[i + 1]);
        }

        currentNode = null;
        GlobalCount = 0;

        //Test 12 - test getbracketText HARD
        string[] test12 = { "(", "5", "+", "(", "6", "-", "x", ")", ")" };
        GlobalCount++;
        string[] s2 = getBracketText(test12);
        for (int i = 0; i < s2.Length; i++)
        {
            Debug.Assert(s2[i] == test12[i + 1]);
        }
        
        currentNode = null;
        GlobalCount = 0;
        //Test 13 - brackets with basic addition
        string[] Test13 = {"(","5","+","6",")","+","5"};
        Node Node13 = ParseNodeTree(Test13);
        Debug.Assert(Node13.evaluate(2) == 16);
        print(Node13.evaluate(2) + ": pass");

        /////////////////ok lets stop FUCKING AROUND
        currentNode = null;
        GlobalCount = 0;
        //Test 14 - brackets with addition and multiplication
        string[] Test14 = { "(", "5", "+", "3", ")", "*", "2" };
        Node Node14 = ParseNodeTree(Test14);
        Debug.Assert(Node14.evaluate(2) == 16);
        print(Node14.evaluate(2) + ": pass");

        currentNode = null;
        GlobalCount = 0;
        //Test 15 - brackets with addition and multiplication
        string[] Test15 = { "20", "-", "(", "x", "+", "6", ")" };
        Node Node15 = ParseNodeTree(Test15);
        Debug.Assert(Node15.evaluate(5) == 9);
        print(Node15.evaluate(5) + ": pass");

        currentNode = null;
        GlobalCount = 0;
        //Test 16 - double brackets
        string[] Test16 = { "20", "-", "(", "x", "*", "(", "3", "+", "x", ")", ")" };
        Node Node16 = ParseNodeTree(Test16);
        Debug.Assert(Node16.evaluate(2) == 10);
        print(Node16.evaluate(2) + ": pass");

        currentNode = null;
        GlobalCount = 0;
        //Test 17 - double brackets-2
        string[] Test17 = { "5", "-", "(", "x", "/", "(", "x", "+", "x", ")", ")" };
        Node Node17 = ParseNodeTree(Test17);
        Debug.Assert(Node17.evaluate(2) == 4.5);
        print(Node17.evaluate(2) + ": pass");

        currentNode = null;
        GlobalCount = 0;
        //Test 18 - double brackets-2
        string[] Test18 = { "sin", "(", "5", "-", "5", ")" };
        Node Node18 = ParseNodeTree(Test18);
        Debug.Assert(Node18.evaluate(2) == 0);
        print(Node18.evaluate(2) + ": pass");

        currentNode = null;
        GlobalCount = 0;
        //Test 19 - misc
        string[] Test19 = { "sin", "(", "1", "-", "1", ")", "+","10"};
        Node Node19 = ParseNodeTree(Test19);
        Debug.Assert(Node19.evaluate(2) == 10);
        print(Node19.evaluate(2) + ": pass");

        currentNode = null;
        GlobalCount = 0;
        //test20 - exponents:1
        string[] Test20 = { "5", "^", "2" };
        Node Node20 = ParseNodeTree(Test20);
        Debug.Assert(Node20.evaluate(2) == 25);
        print(Node20.evaluate(2) + ": pass");

        currentNode = null;
        GlobalCount = 0;

        //test21 - exponents:2
        string[] Test21 = { "x", "^", "3" };
        Node Node21 = ParseNodeTree(Test21);
        Debug.Assert(Node21.evaluate(2) == 8);
        print(Node21.evaluate(2) + ": pass");
        
        currentNode = null;
        GlobalCount = 0;

        //test22 - exponents:3
        string[] Test22 = { "x", "^", "x", "+","3"};
        Node Node22 = ParseNodeTree(Test22);
        Debug.Assert(Node22.evaluate(2) == 7);
        print(Node22.evaluate(2) + ": pass");

        currentNode = null;
        GlobalCount = 0;

        //test23 - cosine
        string[] Test23 = { "cos", "(", "3","-","3",")" };
        Node Node23 = ParseNodeTree(Test23);
        Debug.Assert(Node23.evaluate(2) == 1);
        print(Node23.evaluate(2) + ": pass");


        currentNode = null;
        GlobalCount = 0;

    }
    //runs tests on differentiation capabilities
    public void differentiationTests()
    {
        currentNode = null;
        GlobalCount = 0;
        //Test 1 - test basic addition:
        string[] Test1 = { "5", "*", "x" };
        Node Node1 = ParseNodeTree(Test1);
        Node NodeD1 = Node1.differentiate();
        Debug.Assert(NodeD1.evaluate(2) == 5);
        print(NodeD1.evaluate(0));
        currentNode = null;
        GlobalCount = 0;
        //Test 2 - test basic addition:
        string[] Test2 = { "5", "+", "x" };
        Node Node2 = ParseNodeTree(Test2);
        Node NodeD2 = Node2.differentiate();
        Debug.Assert(NodeD2.evaluate(2) == 1);
        print(NodeD2.evaluate(0));
        currentNode = null;
        GlobalCount = 0;
        //Test 3 - test basic addition:
        string[] Test3 = { "x", "*", "x" };
        Node Node3 = ParseNodeTree(Test3);
        Node NodeD3 = Node3.differentiate();
        Debug.Assert(NodeD3.evaluate(5) == 10);
        print(NodeD3.evaluate(5));
        currentNode = null;
        GlobalCount = 0;
        //Test 4 - test basic addition:
        string[] Test4 = { "x", "/", "(", "x", "*", "5", ")" };
        Node Node4 = ParseNodeTree(Test4);
        Node NodeD4 = Node4.differentiate();
        Debug.Assert(NodeD4.evaluate(20) == 0);
        print(NodeD4.evaluate(20));
        currentNode = null;
        GlobalCount = 0;
        //Test 5 - test basic addition:
        string[] Test5 = { "sin", "(", "x", "*", "5", ")" };
        Node Node5 = ParseNodeTree(Test5);
        Node NodeD5 = Node5.differentiate();
        Debug.Assert(NodeD5.evaluate(0) == 5);
        print(NodeD5.evaluate(0));
        currentNode = null;
        GlobalCount = 0;

    }
    
    public void runGeneralTests()
    {
        Debug.Assert(isNumber("5"));

        Debug.Assert(isNumber("50"));

        Debug.Assert(isNumber("500"));

        GlobalCount = 0;
        string[] testS = { "5" };
        Debug.Assert(getOperand(testS) == 5);

        GlobalCount = 0;
        string[] testS2 = { "50" };
        Debug.Assert(getOperand(testS2) == 50);
        GlobalCount = 0;
        string[] testS3 = { "500" };
        Debug.Assert(getOperand(testS3) == 500);
        GlobalCount = 0;
        string[] testS4 = { "500","+" };
        Debug.Assert(getOperand(testS3) == 500);
        GlobalCount = 0;
        string[] testS5 = { "-10" };
        Debug.Assert(getOperand(testS5) == -10);
        GlobalCount = 0;
        Debug.Assert(ParseNodeTree(testS5).evaluate(0) == -10);
        GlobalCount = 0;
        this.Reset();
    }
    //given a valid function string, returns a node tree representing the function
    public Node ParseNodeTree(string[] fun)
    {
        //for simple functions less than three characters
        if (fun.Length == 1 && fun[0] != "x")
        {
            currentNode = new Operand((float)System.Convert.ToDouble(fun[0]));
            GlobalCount++;
        }
        else if (fun.Length == 1 && fun[0] == "x")
        {
            currentNode = new Variable();
            GlobalCount++;
        }
        else if (fun.Length == 2 && fun[1] != "x" && fun[0] == "-")
        {
            currentNode = new Operand((float)System.Convert.ToDouble(fun[1]) * -1);
            GlobalCount+=2;
        }//situation where this is the first iteration
        else if (currentNode == null)
        {

            Node Child1 = parseNodeType(fun);
            if (GlobalCount == fun.Length)
            {
                return Child1;
            }
            Node Parent = parseNodeType(fun);
            Node Child2 = parseNodeType(fun);
           
            Parent.Child1 = Child1;
            Parent.Child2 = Child2;
            Child1.Parent = Parent;
            Child2.Parent = Parent;
            currentNode = Parent;
            
        }
        else
        {//situation where this isnt the first iteration

            
            Node Parent = parseNodeType(fun);
            Node Child2 = parseNodeType(fun);
            if(isOperator(Parent) == 1)// + or -
            {
                currentNode = currentNode.getHead();
                currentNode.Parent = Parent;

                Parent.Child1 = currentNode;
                Parent.Child2 = Child2;
                Child2.Parent = Parent;

            }
            else if(isOperator(Parent) == 2)// * or /
            {
                Parent.Child1 = currentNode.Child2;
                Parent.Child2 = Child2;
                Parent.Parent = currentNode;
                Child2.Parent = Parent;
                currentNode.Child2.Parent = Parent;
                currentNode.Child2 = Parent;
                currentNode = Parent;
            }
        }
        if(GlobalCount < fun.Length)
        {
            ParseNodeTree(fun);
        }

        Node root = currentNode.getHead();
        header_node header = new header_node(root);
        root.Parent = header;
        return currentNode.getHead();
    }
    //resets the parser object, should be done every time a new tree is desired
    public void Reset()
    {
        GlobalCount = 0;
        currentNode = null;
    }
    //returns a node object corrosponding to the character
    public Node parseNodeType(string[] text)
    {
        try {
            for (int i = 0; i < text.Length; i++)
            {
               // print("" + (text[i]));
            }
            if (text[GlobalCount] == "*")
            {
                GlobalCount++;
                return new Multiply_Operator();
            } else if (text[GlobalCount] == "~")//use '~' instead of - to make dealing with double negatives easier
            {
                GlobalCount++;
                return new Minus_Operator();
            }
            else if (text[GlobalCount] == "+")
            {
                GlobalCount++;
                return new Add_Operator();
            }
            else if (text[GlobalCount] == "/")
            {
                GlobalCount++;
                return new Divide_Operator();
            }
            else if (text[GlobalCount] == "x")
            {
                GlobalCount++;
                return new Variable();
            }
            else if (text[GlobalCount] == "(")
            {
                GlobalCount++;
                Parser newParse = new Parser();
                string[] sss = getBracketText(text);
                Node tree = newParse.ParseNodeTree(sss);
                return tree.getHead();
            }
            else if (text[GlobalCount] == "s")
            {
                GlobalCount += 4;
                Parser newParse = new Parser();
                string[] sss = getBracketText(text);
                Node tree = newParse.ParseNodeTree(sss);
                Sin_Operator sinOp = new Sin_Operator();
                sinOp.Child1 = tree;
                return sinOp;
            }
            else if (text[GlobalCount] == "c")
            {
                GlobalCount += 4;
                Parser newParse = new Parser();
                string[] sss = getBracketText(text);
                Node tree = newParse.ParseNodeTree(sss);
                Cosine_Operator sinOp = new Cosine_Operator();
                sinOp.Child1 = tree;
                return sinOp;
            }
            else if (text[GlobalCount] == "^")
            {
                GlobalCount++;
                return new Exponent_Operator();
            }
            else
            {
                if (text[GlobalCount] == "-")
                {

                    GlobalCount++;
                    float oper = getOperand(text);
                    return new Operand(oper * -1);//return negative number
                }
                else
                {
                    float oper = getOperand(text);
                    return new Operand(oper);
                }
                //if the text is not a number, then it will throw a conversion error
            }
        }catch(System.IndexOutOfRangeException e)
        {
            print(e);
            string inputString = "";
            for(int i = 0; i < text.Length; i++)
            {
                inputString = inputString + text[i];
            }
            print(inputString + " " + text.Length);
            return null;
        }
    }

    //want to return the number represented at the current place in text
    //need to ensure that numbers of more than one digit are passed correctly
    public float getOperand(string[] text)
    {
        string outputString = "";
        while (GlobalCount < text.Length && isNumber(text[GlobalCount]))
        {
            outputString = outputString + text[GlobalCount];
            GlobalCount++;
        }
        return (float)(System.Convert.ToDouble(outputString));
    }
    //returns true if given character can be cast to a number
    public bool isNumber(string s)
    {
        bool isNum = true;
        //print(s);
        try
        {
            System.Convert.ToDouble(s);
        }
        catch (System.FormatException e)
        {
            isNum = false;
        }
        return (isNum);
    }

    //returns the text that exists within brackets
    public string[] getBracketText(string[] text)
    {
        string[] reText = new string[100];
            int size = 0;
            int bracketCount = 0;
            while(GlobalCount < text.Length)
            {
            if (text[GlobalCount] == ")" && bracketCount == 0)
            {
                GlobalCount++;
                break;
            }
            else { 
                if (text[GlobalCount] == ")")
                {
                    bracketCount--;
                }
                else if (text[GlobalCount] == "(")
                {
                    bracketCount++;
                }
                reText[size] = text[GlobalCount];
                size++;
                GlobalCount++;
            }
            }
        string[] retext2 = new string[size];
        for (int i = 0; i < size; i++)
        {
            retext2[i] = reText[i];
        }
        return retext2;
    }
	
    //checks if the given chracter is an operator, returns its bedmas prioroty or 0 if an operand
    public int isOperator(string text)
    {
        if (text == "+" || text == "-") return 1;
        else if (text == "*" || text == "/" || text == "cos") return 2;
        else if (text == "sin" || text == "cos") return 3;
        else return 0;
    }
    //checks if given node object is operator
    public int isOperator(Node n)
    {
        if (n.GetType() == typeof(Add_Operator) || n.GetType() == typeof(Minus_Operator)) return 1;
        else if (n.GetType() == typeof(Multiply_Operator) || n.GetType() == typeof(Divide_Operator) || n.GetType() == typeof(Exponent_Operator)) return 2;
        else if (n.GetType() == typeof(Sin_Operator) || n.GetType() == typeof(Cosine_Operator)) return 3;
        return 0;
    }
    //checks if the given character is an operand
    public bool isOperand(string text)
    {
        return (text != "*" && text != "-" && text != "+" && text != "/" && text != "sin" && text != "^" && text != "cos");
    }
    
}
