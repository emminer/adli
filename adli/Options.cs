using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adli
{
    class Options
    {
        [Option('n', "noheader", DefaultValue = false)]
        public bool Noheader { get; set; }
    }
}
