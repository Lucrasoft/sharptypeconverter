using System.Collections.Generic;

namespace Converter.Emitter
{
    internal class Result
    {
        internal string MainFile { get; set; }
        internal Dictionary<string, TypeScriptDefinitionGroup> DefinitionFiles { get; set; }
    }
}
