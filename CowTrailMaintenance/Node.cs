using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CowTrailMaintenance
{
    public class Node
    {
        public int Id { get; set; }
        public bool visited;
        public Dictionary<int, int> neighbours = new Dictionary<int, int>();
    }
}
