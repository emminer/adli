using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adli
{
    class GPU
    {
        public int Id { get; set; }
        public int AdapterIndex { get; set; }
        public string Name { get; set; }
        public int Bus { get; set; }
        public ADLODNFanControl Fan { get; set; }
        public int Temperature { get; set; }
    }
}
