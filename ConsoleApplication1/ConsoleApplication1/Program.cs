using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            for (var i = 0; new Func<bool>(() => { 
                for(var j = 0; j < 10;j++){
                    Console.WriteLine("\t" +  j);
                }
                int a, b, c;
                a = b = c = 10;
                Console.WriteLine(a + b + c);
                return i < 10;
                 })(); i++)
            {
                Console.WriteLine(i);
                i++;
            }
            Console.ReadLine();
        }
    }
}
