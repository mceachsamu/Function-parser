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
        //the functions test input
        float[] testInput = {1.0f,2.0f,3.0f,4.0f,5.0f,6.0f };
        //the output that will the function will be optimized to
        float[] testOutput = { 1.0f, 8.0f, 27.0f, 64.0f, 125.0f, 216.0f };
        parse.Reset();
        //the base function is the function that all trees start from
        string[] baseFunction = { "1", "+", "1" };
        //creates the population
        Node[] nodes = createPopulation(100, baseFunction);
        //runs the genetic algorithm and returns the most optimized individuals
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
        {//for each iteration mutate each individual, preserving the first k individuals from the previous generation
            for (int j = nodes.Length/k+1; j < nodes.Length; j++)
            {
                nodes[j].Mutate(10, 0.1f);
            }
            //select the best individuals for this generation.
            nodes = selectBestK(k, nodes.Length, nodes, input, output);
        }
        return (nodes);
    }

    
    //selects the best K individuals in the given set and returns a set of 'populationsize' many individuals
    //that are the same as the best k individuals
    public Node[] selectBestK(int k,int populationSize, Node[] nodes, float[] input, float[] output)
    {
        //if for whatever reason k is the size of the population, just return all the nodes
        if(k >= nodes.Length)
        {
            return nodes;
        }
        Node[] bestK = new Node[k];
        int[] indexes = new int[k];
        int count = 0;
        //want to find the k best individuals
        for (int j = 0; j < k; j++)
        {
            float minError = float.PositiveInfinity;
            Node minNode = null;
            int minIndex = -1;
            //iterate through all the nodes, and evaluate the fitness to find the best individual
            for (int i = 0; i < nodes.Length; i++)
            {
                float sumError = 0;
                for(int n = 0; n < input.Length; n++)
                {
                    sumError = sumError + Mathf.Abs(nodes[i].evaluate(input[n]) - output[n]);
                }
                //ensures we are not duplicating individuals in the returning set
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
            count = count + 1;
            bestK[j] = minNode;
        }
        int duplicate = (populationSize / k);
        Node[] result = new Node[populationSize];
        int counter = 0;
        //want to duplicate the best k individuals n/k many times to return a set that is the same size as initial population
        //where n is population size
        for (int i = 0; i < populationSize; i++)
        {
            Parser p = new Parser();
            //convert the tree back to a string, and then back to a node tree to prevent pointer annoyances.
            string bestKString = "0+" + bestK[i/k].asString();
            result[i] = p.ParseNodeTree(toStringArray(bestKString));
            p.Reset();
        }
        return result;
    }

    //converts a string to a string array to help with parsing into a node tree.
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
