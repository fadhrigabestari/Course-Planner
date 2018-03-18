using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoursePlanner
{
    class Node
    {
        // Attributes
        private string course;  // Name of course
        private List<string> requiredFor;   // List of other courses that can be taken after this course
        private int term;   // Course has to be taken on this term

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
            foreach (string v in N.requiredFor)
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
    }
}
