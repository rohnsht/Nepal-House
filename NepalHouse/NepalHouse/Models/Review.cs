using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NepalHouse.Models
{
    public class Review
    {
        public int id { get; set; }
        public string review { get; set; }
        public string date_created { get; set; }
        public int rating { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public bool verified { get; set; }
    }
}
