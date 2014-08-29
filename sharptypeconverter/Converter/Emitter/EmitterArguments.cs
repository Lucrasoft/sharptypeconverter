using Converter.TypeTree;
using ICSharpCode.NRefactory.CSharp;
using System.Collections.Generic;

namespace Converter.Emitter
{
    internal class EmitterArguments
    {
        internal Tree TypeTree { get; set; }
        internal List<string> ReferencedNamespaces { get; set; }
        internal CodeSegment Output { get; set; }
        internal Queue<AstNode> PostPonedNodes { get; set; }
        internal SyntaxTree SyntaxTree { get; set; }
        internal bool IsForStatementInitializerBusy { get; set; }
    }
}
