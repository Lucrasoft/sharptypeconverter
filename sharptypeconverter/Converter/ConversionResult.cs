using System.Collections.Generic;

namespace Converter
{
    public class ConversionResult
    {
        public List<string> MainFiles { get; set; }
        public Dictionary<string, TypeScriptDefinitionGroup> DefinitionFiles { get; set; }

        public ConversionResult()
        {
            MainFiles = new List<string>();
            DefinitionFiles = new Dictionary<string, TypeScriptDefinitionGroup>();
        }
    }
}
