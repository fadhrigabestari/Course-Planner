using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    class Node
    {
        // Attributes
        private string course;   // Name of course
        private List<string> requiredFor;      // List of other courses that can be taken after this course
        private int term; // Course has to be taken on this term

        // Constructors
        public Node() // ctor
        {  
            course = "course";
            requiredFor = new List<string>();
            term = 0;
        }
        public Node(Node N) // cctor
        { 
            course = N.course;
            requiredFor = new List<string>();
            foreach(string v in N.requiredFor)
            {
                requiredFor.Add(v);
            }
            term = N.term;
        }

        //Setter & Getter
        public void setCourse(string v)
        {
            course = v;
        }
        public void addReq(string v)
        {
            requiredFor.Add(v);
        }
        public void setTerm(int n)
        {
            term = n;
        }
        public int getTerm()
        {
            return term;
        }
        public int getLenReq()
        {
            return requiredFor.Count;
        }
        public string getCourse()
        {
            return course;
        }
        public string getReqN(int n)
        {
            return requiredFor[n];
        }
        public List<string> getReq()
        {
            return requiredFor;
        }

        // Method function
        public void delReq(string v) // Delete a course from list of required for
        {
            requiredFor.Remove(v);
        }
        public bool searchReq(string v) // Return true if v is in list of required for
        {
            for (int i = 0; i < getLenReq(); i++)
            {
                if (requiredFor[i] == v)
                {
                    return true;
                }
            }
            return false;
        }
        public int searchReqN(string v) // Return the index of v in required for list
        {
            for (int i = 0; i < getLenReq(); i++)
            {
                if (requiredFor[i] == v)
                {
                    return i;
                }
            }
            return -1;
        }
    }

    class Graph
    {
        // Attributes
        private List<Node> nodes;

        // Constructor
        public Graph() // ctor
        {
            nodes = new List<Node>();
        }
        public Graph(Graph g) // cctor
        {
            nodes = new List<Node>();
            foreach(Node n in g.nodes)
            {
                addNode(n);
            }
        }

        // Setter & Getter
        public List<Node> getNodes()
        {
            return nodes;
        }
        public int getLenNodes()
        {
            return nodes.Count;
        }
        public void setTermGraph(int t, int i)
        {
            nodes[i].setTerm(t);
        }

        // Method function
        public void addNode(Node n) // Add Node n into nodes last
        {
            nodes.Add(n);
        }
        public void addFirstNode(Node n) // Add Node n into nodes first
        {
            nodes.Insert(0, n);
        }
        public void delNode(Node n) // Delete Node n from list of nodes
        {
            nodes.Remove(n);
        }
        public bool boolsearchNode(string v) // Return true if v is in nodes
        {
            foreach(Node n in nodes)
            {
                if(n.getCourse() == v)
                {
                    return true;
                }
            }
            return false;
        }
        public Node searchNode(string v) // Return the node with course name v in nodes
        {
            foreach(Node n in nodes)
            {
                if(n.getCourse() == v)
                {
                    return n;
                }
            }
            return nodes[0];
        }
        public Graph noPreReq() // Return a graph of all nodes without prerequisite
        {
            Graph graph = new Graph();
            foreach (Node n in nodes)
            {
                int requirement = 0;
                foreach (Node m in nodes)
                {
                    if (n != m)
                    {
                        for (int i = 0; i < m.getLenReq(); i++)
                        {
                            if (n.getCourse() == m.getReqN(i))
                            {
                                requirement++;
                            }
                        }
                    }
                }
                if(requirement == 0)
                {
                    graph.addNode(n);
                }
            }
            return graph;
        }
        public Node pickStartNode(Graph doneGraph) // Choose a start node to do DFS
        {
            foreach(Node n in nodes)
            {
                if(!doneGraph.boolsearchNode(n.getCourse()))
                {
                    return n;
                }
            }
            return nodes[0];
        }
        public bool isAllReqDone(Node currentNode) // Return true of all of course in required for list is done
        {
            foreach(string v in currentNode.getReq())
            {
                if(!boolsearchNode(v))
                {
                    return false;
                }
            }
            return true;
        }
        public List<Graph> seperateTerm() // Return a list of graph that has been seperated by its courses' terms
        {
            List<Graph> final = new List<Graph>();
            Graph graph = new Graph(this);
            while (graph.getLenNodes() != 0)
            {
                int currentTemp = graph.getNodes()[0].getTerm();
                Graph temp = new Graph();
                foreach (Node n in graph.nodes)
                {
                    if (n.getTerm() == currentTemp)
                    {
                        temp.addNode(n);
                    }
                }
                foreach (Node n in temp.nodes)
                {
                    graph.delNode(n);
                }
                final.Add(temp);
            }
            return final;
        }
        public void printGraph() // Print initial graph
        {
            Console.WriteLine("\nGraph : ");
            foreach (Node n in nodes)
            {
                Console.Write("Course : "); Console.WriteLine(n.getCourse());
                for (int i = 0; i < n.getLenReq(); i++)
                {
                    Console.Write("Required for : "); Console.WriteLine(n.getReqN(i));
                }
            }
        }
        public void BFS(Graph BFSgraph) // Delete the nodes without prerequisite until graph is empty
        {
            Graph graph = new Graph(this);
            int i = 1;
            while(graph.getLenNodes() != 0)
            {
                Graph noPrereqNode = graph.noPreReq();
                foreach(Node n in noPrereqNode.getNodes())
                {
                    n.setTerm(i);
                    BFSgraph.addNode(n);
                    graph.delNode(n);
                    Console.Write(n.getCourse()); Console.WriteLine(" is visited.");
                }
                i++;
            }
        }
        public void DFS(Node currentNode, Graph DFSgraph) // Use topological sort
        {
            Console.Write(currentNode.getCourse()); Console.WriteLine(" is visited");
            if(currentNode.getLenReq() == 0)
            {
                DFSgraph.addFirstNode(currentNode);
                Console.Write(currentNode.getCourse()); Console.WriteLine(" is done");
            } else
            {
                if(DFSgraph.isAllReqDone(currentNode))
                {
                    DFSgraph.addFirstNode(currentNode);
                    Console.Write(currentNode.getCourse()); Console.WriteLine(" is done");
                } else
                {
                    foreach (string v in currentNode.getReq())
                    {
                        if(!DFSgraph.boolsearchNode(v))
                        {
                            Node newCurrentNode = searchNode(v);
                            DFS(newCurrentNode, DFSgraph);
                        }
                    }
                    DFSgraph.addFirstNode(currentNode);
                    Console.Write(currentNode.getCourse()); Console.WriteLine(" is done");
                }
            }
        }
    }

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
                while (DFSgraph.getLenNodes() != graph.getLenNodes())
                {
                    Node startNode = graph.pickStartNode(DFSgraph);
                    graph.DFS(startNode, DFSgraph);
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
