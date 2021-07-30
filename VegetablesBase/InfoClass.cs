using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VegetablesBase
{
    public class InfoClass
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public InfoClass(int id = 0, string name = "")
        {
            ID = id;
            Name = name;
        }
        //public GoodsType() { }
    }
}
