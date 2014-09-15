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
                File.WriteAllText(path + ".ts", output.MainFiles.First());
                var libraryPath = Path.GetDirectoryName(path) + "\\Library";
                Directory.CreateDirectory(libraryPath);
                foreach (var file in output.DefinitionFiles)
                {
                    File.WriteAllText(libraryPath + "\\" +  file.Key +".d.ts", file.Value.ToString());
                }
            }
            if (Directory.Exists(path))
            {
                var files = Directory.EnumerateFiles(path).Where(f => Path.GetExtension(f).Equals(".cs")).ToList();
                var codes = files.Select(File.ReadAllText);
                var result = Converter.Converter.Convert(codes.ToList());
                var newDirectpryPath = path + "Typescript";
                Directory.CreateDirectory(newDirectpryPath);
                for (var i = 0; i < result.MainFiles.Count(); i++)
                {
                    var fileName = Path.GetFileNameWithoutExtension(files.ElementAt(i));
                    File.WriteAllText(newDirectpryPath + "\\" + fileName + ".ts", result.MainFiles.ElementAt(i));
                }
                Directory.CreateDirectory(newDirectpryPath + "\\Library");
                foreach (var file in result.DefinitionFiles)
                {
                    File.WriteAllText(newDirectpryPath + "\\Library\\" + file.Key + ".d.ts", file.Value.ToString());
                }
            }
        }
    }
}