using System;
using System.IO;
using System.Linq;

namespace sharptypeconverter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("enter file or folder location");
            var path = Console.ReadLine();
            if (path == null)
            {
                return;
            }
            if (File.Exists(path))
            {
                var output = Converter.Converter.Convert(File.ReadAllText(path));
                File.WriteAllText(path + ".ts", output);
            }
            if (Directory.Exists(path))
            {
                var files = Directory.EnumerateFiles(path).Where(f => Path.GetExtension(f).Equals(".cs")).ToList();
                var codes = files.Select(File.ReadAllText);
                var typescriptCodes = Converter.Converter.Convert(codes.ToList()).ToList();
                var newDirectpryPath = path + "Typescript";
                Directory.CreateDirectory(newDirectpryPath);
                for (var i = 0; i < typescriptCodes.Count(); i++)
                {
                    var fileName = Path.GetFileNameWithoutExtension(files.ElementAt(i));
                    File.WriteAllText(newDirectpryPath + "\\" + fileName + ".ts", typescriptCodes.ElementAt(i));
                }
            }
        }
    }
}