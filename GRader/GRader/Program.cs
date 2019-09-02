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
            var reader = new Parser("correction.xml");
            reader.Parse();
            if (reader.IsConsistent())
            {
                reader.DisplayComments();
                reader.DisplayGrade();
            }
            //Console.ReadKey();
        }
    }
}
