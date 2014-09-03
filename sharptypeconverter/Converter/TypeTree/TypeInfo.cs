using ICSharpCode.NRefactory.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter.TypeTree
{
    public class TypeInfo
    {
        public string TypeName;
        public ClassType classType;
        public List<string> allNames;
        public Dictionary<string, TIMethod> Methods;
        public Dictionary<string, TIConstructor> Constructors;
        public Dictionary<string, TIOperator> Operators;

        public bool NameExists(string name)
        {
            return (allNames.IndexOf(name) != -1);
        }

        public TypeInfo(string typeName, ClassType classType)
        {
            this.allNames = new List<string>();
            this.Methods = new Dictionary<string, TIMethod>();
            this.Constructors = new Dictionary<string, TIConstructor>();
            this.Operators = new Dictionary<string, TIOperator>();
            this.TypeName = typeName;
            this.classType = classType;
        }

        public void AddConstructor(ConstructorDeclaration cd)
        {
            var name = cd.Name;
            this.allNames.Add(name);
            if (!Constructors.ContainsKey(name))
            {
                Constructors.Add(name, new TIConstructor());
            }
            var constructor = Constructors[name];
            constructor.addDeclaration(cd);
        }


        public void AddMethod(MethodDeclaration md)
        {
            var name = md.Name;
            allNames.Add(name);
            if (!Methods.ContainsKey(name))
            {
                Methods.Add(name, new TIMethod());
            }
            var method = Methods[name];
            method.addDeclaration(md);



        }

        public void AddProperty(PropertyDeclaration pd)
        {
            allNames.Add(pd.Name);
        }

        public void AddField(FieldDeclaration fd)
        {
            foreach (var item in fd.Variables)
            {
                allNames.Add(item.Name);
            }

        }
        public void AddOperator(OperatorDeclaration od)
        {
            var name = od.OperatorType.ToString();
            if (!Operators.ContainsKey(name))
            {
                Operators.Add(name, new TIOperator());
            }
            var operat = Operators[name];
            operat.addDeclaration(od);
        }

    }
}
