using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharptypeconverter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("enter file location");
            var file = Console.ReadLine();
            
            var output = Converter.Converter.Convert(System.IO.File.ReadAllText(file));
            System.IO.File.WriteAllText(file + ".ts", output);

        }
    }
}
