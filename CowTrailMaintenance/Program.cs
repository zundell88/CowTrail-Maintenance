using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CowTrailMaintenance
{
    class Program
    {
        public static Node[] nodeArray;
        public static int amountNodes;
        public static Random rnd = new Random();
        public static bool AllAdded => nodeArray.All(n => n != null);

        static void Main(string[] args)
        {
            Console.Write($"Skriv in antal (hörn, kanter): ");
            var firstInput = Console.ReadLine().Split(' ');
            nodeArray = new Node[int.Parse(firstInput[0])]; // bygger Node-array med input nr 1
            amountNodes = int.Parse(firstInput[0]);

            for (int i = 0; i < int.Parse(firstInput[1]); i++) // körs efter input nr2 
            {
                Console.Write($"Ge input: ");
                var input = Console.ReadLine().Split(' ');

                AddNodeToArray(input);

                if (!AllAdded)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"-1");
                    Console.ResetColor();
                }
                if (AllAdded)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(PrimAlgorithm());
                    Console.ResetColor();
                }
            }
        }

        public static string PrimAlgorithm()
        {
            int totalWeight = 0;
            List<Node> visitedNodes = new List<Node>();
            var startNode = nodeArray[rnd.Next(nodeArray.Length - 1)];
            startNode.visited = true;
            visitedNodes.Add(startNode);
            while (visitedNodes.Count != amountNodes)
            {
                int lowestWeight = 0;
                int nodeId = 0;

                //Check for the edge with the lowest weight from all visited nodes
                foreach (var node in visitedNodes)
                {
                    foreach (var neighbour in node.neighbours)
                    {
                        if (nodeArray[neighbour.Key - 1].visited == false)
                        {
                            var currentWeight = neighbour.Value;
                            if (currentWeight < lowestWeight || lowestWeight == 0)
                            {
                                lowestWeight = currentWeight;
                                nodeId = neighbour.Key;
                            }
                        }
                    }
                }

                //After finding the edge add to counter and mark node as visited
                totalWeight += lowestWeight;
                var nextNode = nodeArray[nodeId - 1];
                nextNode.visited = true;
                visitedNodes.Add(nextNode);
            }

            foreach (var node in nodeArray)
            {
                node.visited = false;
            }

            return $"{totalWeight}";
        }

        public static void AddNodeToArray(string[] input)
        {
            if (input.Length > 3)
            {
                throw new ArgumentOutOfRangeException("Input innehöll för många tecken");
            }
            int from = int.Parse(input[0]);
            int to = int.Parse(input[1]);
            int weight = int.Parse(input[2]);

            if (weight == 0)
            {
                throw new ArgumentOutOfRangeException("Vikt kan inte vara noll");
            }

            // Hämtar noden från array
            Node nodeFrom = nodeArray[from - 1];
            Node nodeTo = nodeArray[to - 1];

            // Kolla From-noden
            if (nodeFrom != null)
            {
                int oldWeight;
                //kolla om det finns en kant mellan från och till
                nodeArray[nodeFrom.Id - 1].neighbours.TryGetValue(to, out oldWeight);

                //finns det inte skapa till noden
                if (oldWeight == 0)
                {
                    if (nodeTo == null)
                    {
                        nodeTo = new Node();
                        nodeTo.Id = to;
                        nodeTo.neighbours.Add(from, weight);
                        nodeArray[nodeTo.Id - 1] = nodeTo;
                        nodeArray[nodeFrom.Id - 1].neighbours.Add(to, weight);
                    }
                    else
                    {
                        nodeTo.neighbours.Add(from, weight); // lägg till från-noden till till-nodens grannar
                        nodeArray[nodeTo.Id - 1] = nodeTo;
                        nodeArray[nodeFrom.Id - 1].neighbours.Add(to, weight); //Lägg till nya grannen med vikt
                    }
                }
                else if (weight < oldWeight)
                    nodeFrom.neighbours[to] = weight;
            }
            else
            {
                nodeFrom = new Node();
                nodeFrom.Id = from;
                nodeFrom.neighbours.Add(to, weight);
                nodeArray[nodeFrom.Id - 1] = nodeFrom;
            }

            // Kolla To-noden
            if (nodeTo != null)
            {
                int oldWeight;
                nodeArray[nodeTo.Id - 1].neighbours.TryGetValue(from, out oldWeight);

                if (oldWeight == 0)
                {
                    nodeFrom = new Node();
                    nodeFrom.Id = from;
                    nodeFrom.neighbours.Add(to, weight);
                    nodeArray[nodeFrom.Id - 1] = nodeFrom;
                    nodeArray[nodeTo.Id - 1].neighbours.Add(from, weight);
                }

                else if (weight < oldWeight)
                    nodeTo.neighbours[from] = weight;
            }
            else
            {
                nodeTo = new Node();
                nodeTo.Id = to;
                nodeTo.neighbours.Add(from, weight);
                nodeArray[nodeTo.Id - 1] = nodeTo;
            }
        }
    }
}
