using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    private string startFuntion = "0+(x+cos(1))";
    private Node node;
    // Start is called before the first frame update
    void Start()
    {
        char[] function = startFuntion.ToCharArray();
        string[] functionString = new string[function.Length];
        for (int i = 0; i < function.Length; i++)
        {
            functionString[i] = function[i].ToString();
        }
        Parser parse = new Parser();
        //node points the the root node of the tree
        node = parse.ParseNodeTree(functionString);
        node.Parent = new header_node(node);
        //should return 2
        //the evaluate parameter is the input variable
        float[] testInput = {1.0f,2.0f,3.0f,4.0f,5.0f,6.0f };
        float[] testOutput = { 1.0f, 8.0f, 27.0f, 64.0f, 125.0f, 216.0f };
        parse.Reset();
        string[] baseFunction = { "1", "+", "1" };
        Node[] nodes = createPopulation(100, baseFunction);
        Node[] best = GeneticAlg(nodes, 10, testInput, testOutput, 10);
        for(int i = 0; i < best.Length; i++)
        {
            print("best: " + best[i].asString() + best[i].evaluate(5));
        }
    }

    //returns a population of basic 1+1 functions represented by node trees, where each node
    //in the array is a root node of each tree
    public Node[] createPopulation(int size, string[] baseFunction)
    {
        Node[] nodes = new Node[size];
        for(int i = 0; i < size; i++)
        {
            Parser parse = new Parser();
            nodes[i] = parse.ParseNodeTree(baseFunction);
            parse.Reset();
        }
        return nodes;
    }

    public Node[] GeneticAlg(Node[] nodes, int iterations, float[] input, float[] output, int k)
    {
        Node[] preserve = new Node[iterations];
        for(int i = 0; i < iterations; i++)
        {
            for (int j = nodes.Length/k+1; j < nodes.Length; j++)
            {
                nodes[j].Mutate(10, 0.1f);
                //print("----> " + nodes[j].asString());
            }
            nodes = selectBestK(k, nodes.Length, nodes, input, output);
        }
        return (nodes);
    }

    
    //selects the best K individuals in the given set and returns a set of 'populationsize' many individuals
    //that are the same as the best k individuals
    public Node[] selectBestK(int k,int populationSize, Node[] nodes, float[] input, float[] output)
    {
        if(k >= nodes.Length)
        {
            return nodes;
        }
        Node[] bestK = new Node[k];
        int[] indexes = new int[k];
        int count = 0;
        for (int j = 0; j < k; j++)
        {
            float minError = float.PositiveInfinity;
            Node minNode = null;
            int minIndex = -1;
            
            for (int i = 0; i < nodes.Length; i++)
            {
                //print(nodes[i].asString());
                float sumError = 0;
                for(int n = 0; n < input.Length; n++)
                {
                    sumError = sumError + Mathf.Abs(nodes[i].evaluate(input[n]) - output[n]);
                }
                bool isAlreadyIn = false;
                for(int p = 0; p < k; p++)
                {
                    if(i == indexes[p])
                    {
                        isAlreadyIn = true;
                    }
                }
                if(sumError < minError && !isAlreadyIn)
                {
                    minError = sumError;
                    minNode = nodes[i];
                    minIndex = i;
                }
            }
            indexes[count] = minIndex;
            //print(minNode.asString() + " " + minError);
            count = count + 1;
            bestK[j] = minNode;
        }
        int duplicate = (populationSize / k);
        Node[] result = new Node[populationSize];
        int counter = 0;
        for (int i = 0; i < populationSize; i++)
        {
            Parser p = new Parser();
            string bestKString = "0+" + bestK[i/k].asString();
            result[i] = p.ParseNodeTree(toStringArray(bestKString));
            p.Reset();
        }
        for (int i = 0; i < k; i++)
        {
            //print(result[i].asString());
        }
        return result;
    }

    public Node mutateNodeTree(Node rootNode, float probability, int range)
    {
        rootNode.Mutate(range, probability);
        return (rootNode);
    }

    public string[] toStringArray(string input)
    {
        char[] function = input.ToCharArray();
        string[] functionString = new string[function.Length];
        for (int i = 0; i < function.Length; i++)
        {
            functionString[i] = function[i].ToString();
        }
        return (functionString);
    }
}
