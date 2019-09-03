using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRader
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser reader = null;
            if (args.Length > 0)
            {
                reader = new Parser(args[0]);
            } else
            {
                reader = new Parser("correction.xml");
            }

            reader.Parse();
            if (reader.IsConsistent())
            {
                reader.DisplayComments();
                reader.DisplayGrade();
            }
            Console.ReadKey();
        }
    }
}
