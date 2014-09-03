using ICSharpCode.NRefactory.CSharp;
using Converter.TypeTree;
using Converter.Emitter.OutputFunctions.HelperFunctions;

namespace Converter.Emitter.OutputFunctions
{
    internal class PropertyDeclarationEmitter : BaseEmitter<PropertyDeclaration>
    {
        readonly CodeSegment output;
        readonly Tree typeTree;
        internal PropertyDeclarationEmitter(EmitterArguments arguments)
            : base(arguments)
        {
            output = arguments.Output;
            typeTree = arguments.TypeTree;
        }
        internal override void Output(PropertyDeclaration node)
        {
            bool isPrivate = ((node.Modifiers & Modifiers.Private) == Modifiers.Private);
            if (typeTree.ActiveType.classType == ClassType.Interface)
            {
                output.AddWithoutSpace(node.Name);
                output.Add(":");
                output.AddWithoutSpace(AstTypeToStringConverter.Convert(node.ReturnType,EmitterArguments));
                output.Add(";");
                output.NewLine();
            } //not an interface, so proceed with normal property processing.
            else
            {

                //Check if getter and/or setter has no Body , then an implicit {get;set;} or {get} is made
                //We need to make a field whenever it is implicit defined

                bool isImplicitProperty = false;
                if (!node.Getter.IsNull) { isImplicitProperty = isImplicitProperty | (node.Getter.Body.IsNull); }
                if (!node.Setter.IsNull) { isImplicitProperty = isImplicitProperty | (node.Setter.Body.IsNull); }

                if (isImplicitProperty)
                {
                    output.NewLine();
                    output.Add("private");
                    output.AddWithoutSpace("__" + node.Name);
                    output.Add(":");
                    output.AddWithoutSpace(AstTypeToStringConverter.Convert(node.ReturnType,EmitterArguments));
                    output.Add(";");
                }


                if (!node.Getter.IsNull)
                {
                    output.NewLine();
                    if (isPrivate) { output.Add("private"); }
                    output.Add("get");
                    output.Add(node.Name + "()");
                    output.Add(":");
                    output.Add(AstTypeToStringConverter.Convert(node.ReturnType,EmitterArguments));
                    output.Add("{");
                    output.NewLine();
                    output.IndentIncrease();
                    //
                    if (node.Getter.Body.IsNull)
                    {
                        output.Add("return");
                        output.AddWithoutSpace("this.__" + node.Name);
                        output.Add(";");
                        output.NewLine();
                    }
                    else
                    {
                        new BlockStatementEmitter(EmitterArguments).Output(node.Getter.Body);
                    }
                    output.IndentDecrease();
                    output.Add("}");
                }

                if (!node.Setter.IsNull)
                {
                    output.NewLine();
                    if (isPrivate) { output.Add("private"); }
                    output.Add("set");
                    output.Add(node.Name + "(value : ");
                    output.Add(AstTypeToStringConverter.Convert(node.ReturnType,EmitterArguments));
                    output.Add(")");

                    // value : type    maybe better ?!
                    output.Add("{");
                    output.NewLine();
                    output.IndentIncrease();
                    //
                    if (node.Setter.Body.IsNull)
                    {
                        output.Add("this.__" + node.Name);
                        output.Add("=");
                        output.Add("value;");
                        output.NewLine();
                    }
                    else
                    {
                        new BlockStatementEmitter(EmitterArguments).Output(node.Setter.Body);
                    }
                    output.IndentDecrease();
                    output.Add("}");
                }
            }
        }

    }
}
