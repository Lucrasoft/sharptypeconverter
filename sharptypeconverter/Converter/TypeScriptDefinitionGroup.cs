using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Converter
{
    public class TypeScriptDefinitionGroup
    {
        public List<TypeScriptDefinition> Definitions { get; set; }

        public TypeScriptDefinitionGroup()
        {
            Definitions = new List<TypeScriptDefinition>();
        }

        public void AddIfNew(TypeScriptDefinition definition)
        {
            if (!Definitions.Any(x => x.FullName.Equals(definition.FullName)))
            {
                Definitions.Add(definition);
            }
        }
        public void MergeWith(TypeScriptDefinitionGroup group)
        {
            foreach (var definition in group.Definitions)
            {
                AddIfNew(definition);
            }
        }
        public override string ToString()
        {
            var typeDeclaration = new StringBuilder();
            foreach (var definition in Definitions)
            {
                typeDeclaration.AppendLine(definition.ToString());
            }
            return typeDeclaration.ToString();
        }
    }
}
