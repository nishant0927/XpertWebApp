using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Location
{
    public class LocationModel
    {
        public List<Location> Loving { get; set; }
    }
    public class Location
   {
        public string Code { get; set; }
        public string Location_Name { get; set; }
        public bool Result { get; set; }
    }
}
