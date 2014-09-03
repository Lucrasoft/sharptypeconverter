using ICSharpCode.NRefactory.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter.TypeTree
{
    public class TIBase
    {
        public bool isOverloaded
        {
            get
            {
                return declarations.Count > 1;
            }
        }

        public List<NodeReference> declarations;

        public TIBase()
        {
            this.declarations = new List<NodeReference>();
        }

        public void addDeclaration(AstNode node)
        {
            var nr = new NodeReference();
            nr.Line = node.StartLocation.Line;
            nr.Column = node.StartLocation.Column;
            nr.Index = this.declarations.Count;
            this.declarations.Add(nr);
        }

        
    }
}
