using System.Collections.Generic;
using Converter.TypeTree;
using ICSharpCode.NRefactory.CSharp;

namespace Converter
{
    public static class Converter
    {
        public static string Convert(string code){

            var parser = new CSharpParser();
            var typeTree = new Tree();
            var emitter = new Emitter.Emitter(typeTree);
            var typeTreeExtractor = new TypeTreeExtractor(typeTree);
            var syntax = parser.Parse(code);
            typeTreeExtractor.Process(syntax);
            emitter.Process(syntax);
            return emitter.Result;
        }
        public static IEnumerable<string> Convert(List<string> codes){
            var parser = new CSharpParser();
            var typeTree = new Tree();
            var emitter = new Emitter.Emitter(typeTree);
            var typeTreeExtractor = new TypeTreeExtractor(typeTree);

            //preprocess -> fill typeTree with information
            foreach (var code in codes)
            {
                var content = System.IO.File.ReadAllText(code);
                var syntax = parser.Parse(content);
                typeTreeExtractor.Process(syntax);
            }
            var returnList = new List<string>();
            //preprocess -> fill typeTree with information
            foreach (var code in codes)
            {
                var content = System.IO.File.ReadAllText(code);
                var syntax = parser.Parse(content);
                emitter.Process(syntax);
               returnList.Add(emitter.Result);
            }
            return returnList;
        }
    }
}
