using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace adli
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            var isValid = CommandLine.Parser.Default.ParseArgumentsStrict(args, options);
            using (var runner = new ADLRunner())
            {
                runner.Run();
                if (runner.GetGPUList() != null)
                {
                    Print(runner.GetGPUList(), options.Noheader);
                }
                else
                {
                    Console.Error.WriteLine("Failed to get AMD GPU.");
                }
            }
        }

        static void Print(List<GPU> list, bool noheader)
        {
            if (list != null)
            {
                if (!noheader)
                {
                    Console.WriteLine("index, name, temperature, fan");
                }
                for (var i = 0; i < list.Count; i++)
                {
                    Console.WriteLine("{0}, {1}, {2:0.0}, {3}", i, list[i].Name, (float)list[i].Temperature / 1000, list[i].Fan.iCurrentFanSpeed);
                }
            }
        }
    }
}
