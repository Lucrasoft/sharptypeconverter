using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Converter
{
    public class TypeScriptDefinition
    {
        public List<string> Modules { get; set; }
        public string TypeName { get; set; }

        public string FullName
        {
            get { return Modules.Aggregate((current, next) => current + "." + next) + "." + TypeName; }
        }

        public TypeScriptDefinition()
        {
            Modules = new List<string>();
        }
        public override string ToString()
        {
            var typeDeclaration = new StringBuilder("declare ");
            for (var i = 0; i < Modules.Count(); i++)
            {
                typeDeclaration.AppendLine("module " + Modules.ElementAt(i) + "{");
            }
            typeDeclaration.AppendLine("class " + TypeName + "{ \n }");
            for (var i = 0; i < Modules.Count(); i++)
            {
                typeDeclaration.AppendLine("}");
            }
            return typeDeclaration.ToString();
        }
    }
}
