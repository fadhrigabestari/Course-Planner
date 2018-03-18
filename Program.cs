using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoursePlanner
{
    class Program
    {
        static Graph convertFile(string filename) // Conver file into a graph
        {
            Console.WriteLine("File : ");
            List<List<string>> linesList = new List<List<string>>();
            string line;
            char[] coma = new char[] { ',', ' ' };

            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader(filename);
            while ((line = file.ReadLine()) != null)
            {
                List<string> lines = new List<string>();
                lines.AddRange(line.Split(coma, StringSplitOptions.RemoveEmptyEntries));
                lines[lines.Count - 1] = (lines[lines.Count - 1].Split('.'))[0];
                linesList.Add(lines);
                Console.WriteLine(line);
            }
            file.Close();

            Graph graph = new Graph();
            for (int i = 0; i < linesList.Count; i++)
            {
                Node n = new Node();
                n.setCourse(linesList[i][0]);
                for (int j = 0; j < linesList.Count; j++)
                {
                    for (int k = 1; k < linesList[j].Count; k++)
                    {
                        if (n.getCourse() == linesList[j][k])
                        {
                            n.addReq(linesList[j][0]);
                        }
                    }
                }
                graph.addNode(n);
            }
            return graph;
        }
        static void printFinal(List<Graph> result) // Print the final result
        {
            int i = 1;
            foreach(Graph g in result)
            {
                Console.Write("Semester "); Console.Write(i); Console.Write(" : ");
                foreach(Node n in g.getNodes())
                {
                    Console.Write(n.getCourse()); Console.Write(" ");
                }
                Console.WriteLine();
                i++;
            }
        }
        static void Main(string[] args) // Main program
        {
            Console.WriteLine("Welcome to course planner!");
            Console.Write("Input the filename with course prerequisite information : ");
            string filename = Console.ReadLine();
            Graph graph = convertFile(filename);
            graph.printGraph();

            Console.WriteLine("\nGraph transverse method");
            Console.WriteLine("1. BFS");
            Console.WriteLine("2. DFS");
            Console.Write("Choose the transverse method : ");
            int choice = Convert.ToInt32(Console.ReadLine());
            List<Graph> result = new List<Graph>();
 
            if (choice == 1)
            {
                Console.WriteLine();
                Graph BFSgraph = new Graph();
                graph.BFS(BFSgraph);
                result = BFSgraph.seperateTerm();
                Console.WriteLine();
                foreach (Node n in BFSgraph.getNodes())
                {
                    Console.Write(n.getCourse()); Console.Write(" ");
                }
                Console.WriteLine();
            } else
            {
                Console.WriteLine();
                Graph DFSgraph = new Graph();
                Graph startNodes = graph.noPreReq();
                foreach(Node n in startNodes.getNodes())
                {
                    graph.DFS(n, DFSgraph);
                }
                for (int i = 0; i < DFSgraph.getLenNodes(); i++)
                {
                    Node current = DFSgraph.getNodes()[i];
                    int max = current.getTerm();
                    for (int j = 0; j < i; j++)
                    {
                        Node check = DFSgraph.getNodes()[j];
                        foreach (string v in check.getReq())
                        {
                            if (current.getCourse() == v)
                            {
                                if (max < check.getTerm())
                                {
                                    max = check.getTerm();
                                }
                            }
                        }
                    }
                    DFSgraph.setTermGraph(max + 1, i);
                }
                Console.WriteLine();
                result = DFSgraph.seperateTerm();
                foreach (Node n in DFSgraph.getNodes())
                {
                    Console.Write(n.getCourse()); Console.Write(" ");
                }
                Console.WriteLine();
            }

            printFinal(result);
            Console.ReadKey();
        }
    }
}
