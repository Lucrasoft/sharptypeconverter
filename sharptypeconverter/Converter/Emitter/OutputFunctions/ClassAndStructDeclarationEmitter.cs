using System.Linq;
using ICSharpCode.NRefactory.CSharp;
using Modifiers = ICSharpCode.NRefactory.CSharp.Modifiers;

namespace Converter.Emitter.OutputFunctions
{
    internal static class ClassAndStructDeclarationEmitter
    {
        internal static void Output(TypeDeclaration node, EmitterArguments arguments){
            var output = arguments.Output;
            var typeTree = arguments.TypeTree;
            var modifier = (node.Modifiers & Modifiers.Private) == Modifiers.Private ? "private" : "export";
            //check for delegates, they have to by declared outside the class declaration
            var delegates =
                node.Members.Where(m => m.GetType().ToString().Equals("ICSharpCode.NRefactory.CSharp.DelegateDeclaration")).ToList();
            foreach (var delegateNode in delegates)
            {
                new DelegateDeclarationEmitter(arguments, modifier).Output((DelegateDeclaration)delegateNode);
            }
            output.Add(modifier);
            //consider a Struct also as a Class
            output.Add("class");
            output.Add(node.Name);
            //Generic class defintion
            //Example 1 : public class Sample<T>
            //Example 2 : public class Sample<T> where T : stream
            TypeParametersEmitter.Output(node.TypeParameters, node.Constraints,arguments);
            BaseTypesEmitter.Output(node.BaseTypes, true,arguments);
            output.Add("{");
            output.NewLine();
            output.IndentIncrease();
            // members -> proces the node.Members.
            // this should be : fields, properties, constructors and methods
            foreach (EntityDeclaration item in node.Members.Except(delegates))
            {
                Emitter.ProcessNode(item,arguments);
            }
            //Check if overloaded constructor was processed
            if (typeTree.ActiveType.Constructors.ContainsKey(node.Name))
            {
                var constructor = typeTree.ActiveType.Constructors[node.Name];
                if (constructor != null && constructor.isOverloaded)
                {
                    ConstructorOverloadDispatchEmitter.Output(constructor,arguments);
                }
            }
            //Check if overloaded method was processed
            foreach (var methodName in typeTree.ActiveType.Methods.Keys)
            {
                var method = typeTree.ActiveType.Methods[methodName];
                if (method.isOverloaded)
                {
                    //generate a "dispatch" method , which handles/calls all overloaded methods
                    MethodOverloadDispatchEmitter.Output(method,arguments);
                }
            }
            output.IndentDecrease();
            output.Add("}");
        }
    }
}
