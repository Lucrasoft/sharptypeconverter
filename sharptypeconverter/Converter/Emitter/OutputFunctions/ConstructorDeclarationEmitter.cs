using Converter.TypeTree;
using ICSharpCode.NRefactory.CSharp;
using System.Linq;

namespace Converter.Emitter.OutputFunctions
{
    class ConstructorDeclarationEmitter: BaseEmitter<ConstructorDeclaration>
    {
        readonly CodeSegment output;
        readonly Tree typeTree;
        internal ConstructorDeclarationEmitter(EmitterArguments arguments)
            : base(arguments)
        {
            output = arguments.Output;
            typeTree = arguments.TypeTree;
        }

        internal override void Output(ConstructorDeclaration node)
        {
            var cdTree = typeTree.ActiveType.Constructors[node.Name];
            if (cdTree.isOverloaded)
            {
                var index = (from d in cdTree.declarations where (d.Line == node.StartLocation.Line) & (d.Column == node.StartLocation.Column) select d.Index).FirstOrDefault();

                //create "special" constructor
                output.AddWithoutSpace(typeTree.ActiveType.TypeName);
                output.AddWithoutSpace("__" + index);
                ParameterEmitter.Output(node.Parameters, EmitterArguments);
                output.Add("{");
                output.NewLine();
                output.IndentIncrease();

                if (node.Initializer != null)
                {
                    var cdi = node.Initializer;
                    if (!cdi.IsNull)
                    {
                        if (cdi.ConstructorInitializerType == ConstructorInitializerType.Base)
                        {
                            //output.Comment("TODO: Base initilization...");
                            output.AddWithoutSpace("super(");
                            new ArgumentsEmitter(EmitterArguments).Output(cdi.Arguments);
                            output.Add(");");
                            output.NewLine();
                        }

                        if (cdi.ConstructorInitializerType == ConstructorInitializerType.This)
                        {
                            output.Add("this.constructorDispatcher(");
                            new ArgumentsEmitter(EmitterArguments).Output(cdi.Arguments);
                            output.Add(");");
                            output.NewLine();
                        }
                    }
                }
                Emitter.ProcessNode(node.Body, EmitterArguments);

                output.IndentDecrease();
                output.AddLine("}");
                output.NewLine();
            }
            else
            {
                //create "normal" constructor
                output.AddWithoutSpace("constructor");
                ParameterEmitter.Output(node.Parameters, EmitterArguments);
                output.Add("{");
                output.NewLine();
                output.IndentIncrease();


                if (node.Initializer != null)
                {
                    var cdi = node.Initializer;
                    if (!cdi.IsNull)
                    {
                        if (cdi.ConstructorInitializerType == ConstructorInitializerType.Base)
                        {
                            //output.Comment("TODO: Base initilization...");
                            output.AddWithoutSpace("super(");
                            new ArgumentsEmitter(EmitterArguments).Output(cdi.Arguments);
                            output.Add(");");
                            output.NewLine();
                        }

                        if (cdi.ConstructorInitializerType == ConstructorInitializerType.This)
                        {
                            output.Comment("TODO: This initilization..");
                        }
                    }
                }
                Emitter.ProcessNode(node.Body, EmitterArguments);

                output.IndentDecrease();
                output.Add("}");
                output.NewLine();
            }
        }
    }
}
