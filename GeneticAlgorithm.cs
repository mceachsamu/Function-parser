using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    private string startFuntion = "1+1";
    private Node node;
    float[] testInput = { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f, 7.0f, 8.0f, 9.0f, 10.0f };
    float[] testOutput = { -9.0f, -32.0f, -63.0f, -96.0f, -125.0f, -144.0f, -147.0f, -128.0f, -81.0f, 0.0f };
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
        
        //the output that will the function will be optimized to
        
        parse.Reset();
        //the base function is the function that all trees start from
        string[] baseFunction = { "x", "+", "1" };
        //creates the population
        Node[] nodes = createPopulation(500, baseFunction);
        //runs the genetic algorithm and returns the most optimized individuals
        Node[] best = GeneticAlg(nodes, 500, testInput, testOutput, 40, 0.8f);
        for(int i = 0; i < best.Length; i++)
        {
            print(best[i].asString());
        }
        for (int i = 0; i < testInput.Length; i++)
        {
           // print(best[0].evaluate(testInput[i]));
            //print(best[0].asString());
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

    public Node[] GeneticAlg(Node[] nodes, int iterations, float[] input, float[] output, int k, float mutationRate)
    {
        Node[] preserve = new Node[iterations];
        for(int i = 0; i < iterations; i++)
        {//for each iteration mutate each individual, preserving the first k individuals from the previous generation
            for (int j = nodes.Length/k+1; j < nodes.Length; j++)
            {
                nodes[j].Mutate(10, mutationRate);
            }
            //select the best individuals for this generation.
            float[] probabilities = { 0.5f, 0.3f, 0.2f };//sum of probabilities should sum to 1.0f
            nodes = selectBestK(k, nodes.Length, nodes, input, output, probabilities);
        }
        return (nodes);
    }

    
    //selects the best K individuals in the given set and returns a set of 'populationsize' many individuals
    //that are the same as the best k individuals
    public Node[] selectBestK(int k,int populationSize, Node[] nodes, float[] input, float[] output, float[] probabilities)
    {
        Debug.Assert(probabilities.Length == 3);

        Node[] bestK = new Node[k];
        int[] indexes = new int[k];
        int count = 0;
        //the purpose of preserving unfit individuals is to prevent reaching a local minimum and help reach a global maximum
        for (int i = 0; i < k; i++)
        {
            float rand = Random.Range(0.0f, 1.0f);
            if(rand <= probabilities[0])
            {//the first probability corrosponds to the chance of selecting the best individual not yet selected
                bestK[i] = selectBestIndividual(ref indexes, count, nodes, input, output);
            }else if(rand <= probabilities[0] + probabilities[1])
            {//the second probability corrosponds to the chance of selecting an average individual not yet selected
                bestK[i] = selectAverageIndividual(ref indexes, count, nodes, input, output);
            }
            else if(rand <= probabilities[0] + probabilities[1] + probabilities[2])
            {//the third probability corrosponds to the chance of selecting the least fit individual not yet selected
                bestK[i] = selectWorstIndividual(ref indexes, count, nodes, input, output);
            }
            else
            {//if here something went wrong
                print(probabilities[0] + " " + probabilities[1] + " " + probabilities[2] + " " + rand);
                Debug.Assert(false);
            }
        }
        //if for whatever reason k is the size of the population, just return all the nodes
        if(k >= nodes.Length)
        {
            return nodes;
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
            string bestKString = bestK[i/k].asString();
            result[i] = p.ParseNodeTree(toStringArray(bestKString));
            p.Reset();
        }
        return result;
    }



    //select the individual with the highest error
    public Node selectWorstIndividual(ref int[] indexes, int count, Node[] nodes, float[] input, float[] output)
    {
        float maxError = float.NegativeInfinity;
        Node maxNode = null;
        int maxIndex = -1;
        //iterate through all the nodes, and evaluate the fitness to find the best individual
        for (int i = 0; i < nodes.Length; i++)
        {
            float sumError = 0;
            for (int n = 0; n < input.Length; n++)
            {
                sumError = sumError + Mathf.Abs(nodes[i].evaluate(input[n]) - output[n]);
            }
            //ensures we are not duplicating individuals in the returning set
            bool isAlreadyIn = false;
            for (int p = 0; p < indexes.Length; p++)
            {
                if (i == indexes[p])
                {
                    isAlreadyIn = true;
                }
            }
            if (sumError > maxError && !isAlreadyIn)
            {
                maxError = sumError;
                maxNode = nodes[i];
                maxIndex = i;
            }
        }
        indexes[count] = maxIndex;
        count = count + 1;
        float sumE = 0;
        for (int u = 0; u < input.Length; u++)
        {
            sumE = sumE + Mathf.Abs(maxNode.evaluate(input[u]) - output[u]);
        }
        print(maxNode.asString() + " " + sumE);
        return maxNode;
    }

    //select the most individual with the lowest error
    public Node selectBestIndividual(ref int[] indexes, int count, Node[] nodes, float[] input, float[] output)
    {
            float minError = float.PositiveInfinity;
            Node minNode = null;
            int minIndex = -1;
            //iterate through all the nodes, and evaluate the fitness to find the best individual
            for (int i = 0; i < nodes.Length; i++)
            {
                float sumError = 0;
                for (int n = 0; n < input.Length; n++)
                {
                    sumError = sumError + Mathf.Abs(nodes[i].evaluate(input[n]) - output[n]);
                }
                //ensures we are not duplicating individuals in the returning set
                bool isAlreadyIn = false;
                for (int p = 0; p < indexes.Length; p++)
                {
                    if (i == indexes[p])
                    {
                        isAlreadyIn = true;
                    }
                }
                if (sumError < minError && !isAlreadyIn)
                {
                    minError = sumError;
                    minNode = nodes[i];
                    minIndex = i;
                }
            }
            indexes[count] = minIndex;
            count = count + 1;
            float sumE = 0;
            for (int u = 0; u < input.Length; u++)
            {
                sumE = sumE + Mathf.Abs(minNode.evaluate(input[u]) - output[u]);
            }
            print(minNode.asString() + " " + sumE);
        return minNode;
    }

    //selects the individual that is closest to the average error
    public Node selectAverageIndividual(ref int[] indexes, int count, Node[] nodes, float[] input, float[] output)
    {
        float ErrorSum = 0;//overall error
        int countOverall = 0;
        for(int i = 0; i < nodes.Length; i++)
        {
            float sumError = 0;//error for this node
            int countError = 0;
            for(int j = 0; j < input.Length; j++)
            {
                sumError = sumError + Mathf.Abs(nodes[i].evaluate(input[j]) - output[j]);
                countError = countError + 1;
            }
            if(sumError < float.PositiveInfinity)
            {
                ErrorSum = ErrorSum + sumError;
                countOverall = countOverall + 1;
            }
        }
        float averageError = ErrorSum / countOverall;//average error accross all nodes
        float minDifference = float.PositiveInfinity;
        Node averageNode = null;
        int minDiffIndex = 0;
        for(int i = 0; i < nodes.Length; i++)
        {
            float sumError = 0;
            for(int j = 0; j < input.Length; j++)
            {
                sumError = sumError + Mathf.Abs(nodes[i].evaluate(input[j]) - output[j]);
            }
            bool isAlreadyIn = false;
            for (int p = 0; p < indexes.Length; p++)
            {
                if (i == indexes[p])
                {
                    isAlreadyIn = true;
                }
            }
            if (Mathf.Abs(averageError - sumError) < minDifference && !isAlreadyIn)
            {
                minDifference = Mathf.Abs(averageError - sumError);
                averageNode = nodes[i];
                minDiffIndex = i;
            }
        }
        indexes[count] = minDiffIndex;
        count = count + 1;
        float sumE = 0;
        for (int u = 0; u < input.Length; u++)
        {
            sumE = sumE + Mathf.Abs(averageNode.evaluate(input[u]) - output[u]);
        }
        print("averageNode: " +  averageNode.asString() + " " + sumE);
        return averageNode;
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
