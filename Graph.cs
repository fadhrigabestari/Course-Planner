using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoursePlanner
{
    class Graph
    {
        // Attributes
        private List<Node> nodes;
        private static int visit = 0;

        // Constructor
        public Graph() // ctor
        {
            nodes = new List<Node>();
        }
        public Graph(Graph g) // cctor
        {
            nodes = new List<Node>();
            foreach (Node n in g.nodes)
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
        public int getVisit()
        {
            return visit;
        }
        public void setTermGraph(int t, int i)
        {
            nodes[i].setTerm(t);
        }
        public void setVisit(int n)
        {
            visit = n;
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
            foreach (Node n in nodes)
            {
                if (n.getCourse() == v)
                {
                    return true;
                }
            }
            return false;
        }
        public Node searchNode(string v) // Return the node with course name v in nodes
        {
            foreach (Node n in nodes)
            {
                if (n.getCourse() == v)
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
                if (requirement == 0)
                {
                    graph.addNode(n);
                }
            }
            return graph;
        }
        public Node pickStartNode(Graph doneGraph) // Choose a start node to do DFS
        {
            foreach (Node n in nodes)
            {
                if (!doneGraph.boolsearchNode(n.getCourse()))
                {
                    return n;
                }
            }
            return nodes[0];
        }
        public bool isAllReqDone(Node currentNode) // Return true if all courses in required for list is done
        {
            foreach (string v in currentNode.getReq())
            {
                if (!boolsearchNode(v))
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
            setVisit(getVisit() + 1);
            Graph graph = new Graph(this);
            while (graph.getLenNodes() != 0)
            {
                Graph noPrereqNode = graph.noPreReq();
                foreach (Node n in noPrereqNode.getNodes())
                {
                    n.setTerm(getVisit());
                    BFSgraph.addNode(n);
                    graph.delNode(n);
                    Console.Write(n.getCourse()); Console.WriteLine(" is visited.");
                }
                setVisit(getVisit() + 1);
            }
        }
        public void DFS(Node currentNode, Graph DFSgraph) // Use topological sort
        {
            setVisit(getVisit() + 1);
            Console.Write(currentNode.getCourse()); Console.Write(" is visited (");
            Console.Write(getVisit()); Console.WriteLine(")");
            if (currentNode.getLenReq() == 0)
            {
                setVisit(getVisit() + 1);
                DFSgraph.addFirstNode(currentNode);
                Console.Write(currentNode.getCourse()); Console.Write(" is done (");
                Console.Write(getVisit()); Console.WriteLine(")");
            }
            else
            {
                if (DFSgraph.isAllReqDone(currentNode))
                {
                    setVisit(getVisit() + 1);
                    DFSgraph.addFirstNode(currentNode);
                    Console.Write(currentNode.getCourse()); Console.Write(" is done (");
                    Console.Write(getVisit()); Console.WriteLine(")");
                }
                else
                {
                    foreach (string v in currentNode.getReq())
                    {
                        if (!DFSgraph.boolsearchNode(v))
                        {
                            Node newCurrentNode = searchNode(v);
                            DFS(newCurrentNode, DFSgraph);
                        }
                    }
                    setVisit(getVisit() + 1);
                    DFSgraph.addFirstNode(currentNode);
                    Console.Write(currentNode.getCourse()); Console.Write(" is done (");
                    Console.Write(getVisit()); Console.WriteLine(")");
                }
            }
        }
    }
}
