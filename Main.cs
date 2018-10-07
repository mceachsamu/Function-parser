using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {
    public GameObject prefab;
    GameObject main;
    private Node xNode;
    private Node zNode;
    private Node wNode;
    public GameObject measure;
    public GameObject reference;
    
    void Start() {
        //ParseFunctionSet();
        string[] xFunction = { "0.01" ,"*" ,"x","^","2"};
        string[] zFunction = { "0.01", "*", "x", "^", "2" };
        string[] width = { "0.5", "*", "1" };
        
        Parser parse = new Parser();
      //  parse.solvingTests();
        xNode = parse.ParseNodeTree(xFunction);
        parse.Reset();
        zNode = parse.ParseNodeTree(zFunction);
        //print(xNode.solve(zNode));
        parse.Reset();
        wNode = parse.ParseNodeTree(width);
        parse.Reset();
        Mesh m = createFunctionCylander(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
        GameObject g = prefab;
        GameObject gg = (GameObject)Instantiate(g, this.transform.position, Quaternion.identity);
        gg.GetComponent<MeshFilter>().mesh = m;
        Renderer r = gg.GetComponent<Renderer>();
       // gg.AddComponent<MeshCollider>();
        main = gg;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = measure.GetComponent<Transform>().position;
        string xcomp = "" + pos.x;
        string ycomp = "" + pos.y;
        string zcomp = "" + pos.z;
        string[] quadtratic1 = {"1","*","(", "-4","+","(","8","+","4","*","(", xcomp, "+", ycomp, "+"
        ,zcomp,")","^","2",")","^","0.5",")","/","2"};
        string[] quadtratic2 = {"1","*","(", "-4","-","(","8","+","4","*","(", xcomp, "+", ycomp, "+"
        ,zcomp,")","^","2",")","^","0.5",")","/","2"};
        Parser p = new Parser();
        //////////////////////
          Vector3 posR = reference.GetComponent<Transform>().position;
          string sum = "" + (posR.x + posR.y + posR.z);
          string[] sol = {"0", "+","(","1","+","(","1","-","4","*","-0.02","*",sum,")","+","0",")","/","2","*","-0.02"};
         
          Node sol1 = p.ParseNodeTree(sol);
        float ySol = sol1.evaluate(0);

        /////////////////////
       // Node q1 = p.ParseNodeTree(quadtratic1);
        float y = ySol;
        float x = xNode.evaluate(y);
        float z = zNode.evaluate(y);
        Vector3 position = new Vector3(x, y, z);
        reference.GetComponent<Transform>().position = position;//new Vector3(x, y, z);
        print("" + position.y);
    }

    public Mesh ParseFunctionSet()
    {
        Vector3 origin = new Vector3(0, 0, 0);
        ArrayList xSizes = new ArrayList();
        ArrayList zSizes = new ArrayList();
        ArrayList nodesX = new ArrayList();
        ArrayList nodesZ = new ArrayList();
        Parser p = new Parser();

        string[] text = System.IO.File.ReadAllLines("C:/Users/samis/Desktop/other/Level_1.txt");
        for(int i = 0; i < text.Length; i++)
        {
            if (text[i] == "//origin")
            {
                i++;
                char[] c = text[i].ToCharArray();
                origin.x = float.Parse(c[0].ToString());
                origin.y = float.Parse(c[2].ToString());
                origin.z = float.Parse(c[4].ToString());

            }
            else if (text[i] == "//x size")
            {
                i++;
                char[] c = text[i].ToCharArray();
                xSizes.Add(float.Parse(c[0].ToString()));
            }
            else if (text[i] == "//z size")
            {
                i++;
                char[] c = text[i].ToCharArray();
                xSizes.Add(float.Parse(c[0].ToString()));
            }
            else if (text[i] == "//x function")
            {
                i++;
                string[] c = text[i].Split(' ');
                print(text[i]);
                for (int t = 0; t < c.Length; t++)
                {
                    print(" | " + c[t] + " | ");
                }
                nodesX.Add(p.ParseNodeTree(c));
                p.Reset();
            }
            else if (text[i] == "//z function")
            {
                i++;
                string[] c = text[i].Split(' ');
                print(text[i]);
                for (int t = 0; t < c.Length; t++)
                {
                    print(" | " + c[t] + " | ");
                }
                nodesZ.Add(p.ParseNodeTree(c));
                p.Reset();
            }
        }
        return null;
    }

    

    public Mesh createFunctionCylander(Vector3 o, Vector3 rotation)
    {
        Mesh m = new Mesh();
        m.name = "mesh";
        m.Clear();

        int numSegs = 20;
        int heightSegs = 500;
        int yStep = 1;//must divide heightSegs;
      //  int size = ((heightSegs/yStep)*6)
        Vector3[] vs = new Vector3[(int)(((heightSegs/yStep * numSegs) * 6))];
        Vector2[] us = new Vector2[(int)(((heightSegs / yStep * numSegs) * 6))];
        int[] tri = new int[(int)(((heightSegs / yStep * numSegs) * 6))];
        int count = 0;
        for (int y = 0; y < heightSegs; y = y + yStep)
        {
            float z = zNode.evaluate(y);
            float x = xNode.evaluate(y);

            float zP = zNode.evaluate(y - yStep);
            float xP = xNode.evaluate(y - yStep);

            float zP2 = zNode.evaluate(y + yStep);
            float xP2 = xNode.evaluate(y + yStep);
            Vector3 direction = new Vector3(x - xP, y - (y - yStep), z - zP);
            Vector3 direction2 = new Vector3(xP2 - x, (y + yStep) - y, zP2 - z);
            for (int i = 0; i < numSegs; i++)
            {
                Vector3 turn = new Vector3(0, 1, 0);
                float angle = (360 / numSegs) * i;
                turn = Quaternion.AngleAxis(angle, direction) * turn;
                Vector3 norm = Vector3.Cross(direction, turn);
                Vector3 point = new Vector3(0, 0, 0);
                Vector3 dist = norm - point;
                float ch = dist.magnitude;

                float width = wNode.evaluate(y);
                float width2 = wNode.evaluate(y + yStep);
                norm = (norm / norm.magnitude) * width;
                norm.x = norm.x + x;
                norm.z = norm.z + z;
                norm.y = norm.y + y;

                ///////////////////////////////////////
                Vector3 turn2 = new Vector3(0, 1, 0);
                float angle2 = (360 / numSegs) * (i + 1);
                turn2 = Quaternion.AngleAxis(angle2, direction) * turn2;
                Vector3 norm2 = Vector3.Cross(direction, turn2);
                norm2 = (norm2 / norm2.magnitude) * width;
                norm2.x = norm2.x + x;
                norm2.z = norm2.z + z;
                norm2.y = norm2.y + y;
                ////////////////////////////////////////
                Vector3 turn3 = new Vector3(0, 1, 0);
                float angle3 = (360 / numSegs) * i;
                turn3 = Quaternion.AngleAxis(angle3, direction2) * turn3;
                Vector3 norm3 = Vector3.Cross(direction2, turn3);
                Vector3 dist2 = norm3 - point;
                float ch2 = dist2.magnitude;
                norm3 = (norm3 / norm3.magnitude) * width2;
                norm3.x = norm3.x + xP2;
                norm3.z = norm3.z + zP2;
                norm3.y = norm3.y + y + yStep;
                //////////////////////////////////////
                Vector3 turn4 = new Vector3(0, 1, 0);
                float angle4 = (360 / numSegs) * (i + 1);
                turn4 = Quaternion.AngleAxis(angle4, direction2) * turn4;
                Vector3 norm4 = Vector3.Cross(direction2, turn4);
                norm4 = (norm4 / norm4.magnitude) * width2;
                norm4.x = norm4.x + xP2;
                norm4.z = norm4.z + zP2;
                norm4.y = norm4.y + y + yStep;
                ///////////////////////////////////////
                norm = Quaternion.AngleAxis(rotation.y, new Vector3(0, 1, 0)) * norm;
                norm = Quaternion.AngleAxis(rotation.x, new Vector3(1, 0, 0)) * norm;
                norm = Quaternion.AngleAxis(rotation.z, new Vector3(0, 0, 1)) * norm;
                norm2 = Quaternion.AngleAxis(rotation.y, new Vector3(0, 1, 0)) * norm2;
                norm2 = Quaternion.AngleAxis(rotation.x, new Vector3(1, 0, 0)) * norm2;
                norm2 = Quaternion.AngleAxis(rotation.z, new Vector3(0, 0, 1)) * norm2;
                norm3 = Quaternion.AngleAxis(rotation.y, new Vector3(0, 1, 0)) * norm3;
                norm3 = Quaternion.AngleAxis(rotation.x, new Vector3(1, 0, 0)) * norm3;
                norm3 = Quaternion.AngleAxis(rotation.z, new Vector3(0, 0, 1)) * norm3;
                norm4 = Quaternion.AngleAxis(rotation.y, new Vector3(0, 1, 0)) * norm4;
                norm4 = Quaternion.AngleAxis(rotation.x, new Vector3(1, 0, 0)) * norm4;
                norm4 = Quaternion.AngleAxis(rotation.z, new Vector3(0, 0, 1)) * norm4;
                
                vs[count] = norm;
                us[count] = new Vector2(0, 0);
                tri[count] = count;
                count++;

                vs[count] = norm2;
                us[count] = new Vector2(0, 1);
                tri[count] = count;
                count++;
                vs[count] = norm3;
                us[count] = new Vector2(1, 0);
                tri[count] = count;
                count++;
                vs[count] = norm3;
                us[count] = new Vector2(0, 0);
                tri[count] = count;
                count++;
                vs[count] = norm2;
                us[count] = new Vector2(0, 1);
                tri[count] = count;
                count++;
                vs[count] = norm4;
                us[count] = new Vector2(1, 1);
                tri[count] = count;
                count++;
            }
        }
        m.vertices = vs;
        m.uv = us;
        m.triangles = tri;
        m.RecalculateNormals();
        return m;
    }


    public float mod(float a, float b)
    {
        if (a < b)
        {
            return a;
        }
        a = a - b;
        return mod(a, b);
    }

    public Vector3 getComparisonDirection(Vector3 direction)
    {
        if(direction.y < direction.x && direction.y < direction.z)
        {
            return new Vector3(0, 1, 0);
        }else if(direction.x < direction.y && direction.x < direction.z)
        {
            return new Vector3(1, 0, 0);
        }
        else
        {
            return new Vector3(0, 0, 1);
        }
    }


    public Vector3 Colliding(Vector3 position, int numSteps)
    {
        float min = float.PositiveInfinity;
        Vector3 minV = new Vector3(0,0,0);
        float minY = 0;
        for(float i = position.y - numSteps/2; i < position.y + numSteps / 2; i++)
        {
            float x = xNode.evaluate(i);
            float z = zNode.evaluate(i);
            Vector3 comp = new Vector3(x, i, z);
            float dist = Mathf.Abs(comp.magnitude - position.magnitude);
            if(dist < min)
            {
                min = dist;
                minV = comp;
                minY = i;
            }
        }
        float w = wNode.evaluate(minY);
        return minV;
    }

}