using System;
using System.Collections.Generic;
using System.Linq;
using Converter.Emitter;
using Converter.TypeTree;
using ICSharpCode.NRefactory.CSharp;

namespace Converter
{
    public static class Converter
    {
        public static ConversionResult Convert(string code)
        {

            var parser = new CSharpParser();
            var typeTree = new Tree();
            var emitter = new Emitter.Emitter(typeTree);
            var typeTreeExtractor = new TypeTreeExtractor(typeTree);
            var syntax = parser.Parse(code);
            typeTreeExtractor.Process(syntax);
            emitter.Process(syntax);
            return new ConversionResult{MainFiles =  new List<string>{emitter.Result.MainFile},DefinitionFiles = ReferencedTypesEmitter.CreateFiles(emitter.Result.TypesRequested)};
        }
        public static ConversionResult Convert(List<string> codes)
        {
            var parser = new CSharpParser();
            var typeTree = new Tree();
            var emitter = new Emitter.Emitter(typeTree);
            var typeTreeExtractor = new TypeTreeExtractor(typeTree);
            var conversionResult = new ConversionResult();
            //preprocess -> fill typeTree with information
            foreach (var code in codes)
            {
                var syntax = parser.Parse(code);
                typeTreeExtractor.Process(syntax);
            }
            //preprocess -> fill typeTree with information
            var requestedTypes = new Dictionary<string, List<Type>>();
            foreach (var code in codes)
            {
                var syntax = parser.Parse(code);
                emitter.Process(syntax);
                conversionResult.MainFiles.Add(emitter.Result.MainFile);
                foreach (var file in emitter.Result.TypesRequested)
                {
                    if (!requestedTypes.ContainsKey(file.Key))
                    {
                        requestedTypes.Add(file.Key, file.Value);
                    }
                    else
                    {
                        requestedTypes[file.Key] = requestedTypes[file.Key].Union(file.Value).ToList();
                    }
                }
            }
            conversionResult.DefinitionFiles = ReferencedTypesEmitter.CreateFiles(requestedTypes);
            return conversionResult;
        }
    }
}
