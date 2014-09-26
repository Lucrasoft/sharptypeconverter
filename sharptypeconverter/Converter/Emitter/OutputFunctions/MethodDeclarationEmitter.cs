using System;
using System.Collections.Generic;
using System.Globalization;
using Converter.Emitter.OutputFunctions.HelperFunctions;
using Converter.TypeTree;
using ICSharpCode.NRefactory.CSharp;
using System.Linq;
using Mono.CSharp;
using Modifiers = ICSharpCode.NRefactory.CSharp.Modifiers;

namespace Converter.Emitter.OutputFunctions
{
    internal class MethodDeclarationEmitter : BaseEmitter<MethodDeclaration> {
        readonly CodeSegment output;
        readonly Tree typeTree;
        internal MethodDeclarationEmitter(EmitterArguments arguments) : base(arguments)
        {
            output = arguments.Output;
            typeTree = arguments.TypeTree;
        }

        internal override void Output(MethodDeclaration node)
        {
            //Overloaded method should be handled in outputOverloaded function 
            if (typeTree.ActiveType.Methods[node.Name].isOverloaded)
            {
                return;
            }
            OutputMethodHeader(node);

            //If body is null, example when concerning an interface, skip the body
            if (node.Body.IsNull)
            {
                output.Add(";");
            }
            else
            {
                output.AddLine("{");
                output.IndentIncrease();
                new BlockStatementEmitter(EmitterArguments).Output(node.Body);
                output.IndentDecrease();
                output.Add("}");
            }
            output.NewLine();
        }
        internal void OutputOverloaded(List<MethodDeclaration> nodes)
        {
            foreach (var method in nodes)
            {
                OutputMethodHeader(method);
                output.AddLine(";");
            }
            //add main signature
            output.AddLine(nodes.First().Name + "() : any {" );
            output.IndentIncrease();
            output.AddLine("switch (arguments.length) {");
            foreach (var methods in nodes.GroupBy(n => n.Parameters.Count))
            {
                output.AddLine("case " + methods.Key + ":");
                output.IndentIncrease();
                foreach (var method in methods)
                {
                    output.AddWithoutSpace("if(");
                    for (var i = 0; i < methods.Key; i++)
                    {
                        if (i > 0)
                        {
                            output.Add("&&");
                        }
                        output.Add("typeof arguments[" + i + "] == \"" + getJavascriptType(method.Parameters.ElementAt(i).Type.ToString()) + "\"");
                    }
                    output.AddLine("){");
                    output.IndentIncrease();
                    for (var i = 0; i < methods.Key; i++)
                    {
                        output.AddLine("var " + method.Parameters.ElementAt(i).Name + " = arguments[" + i + "];");
                    }
                    new BlockStatementEmitter(EmitterArguments).Output(method.Body);
                    output.IndentDecrease();
                    output.AddLine("}");
                }
                output.IndentDecrease();
                
            }
            output.AddLine("}");
            output.IndentDecrease();
            output.AddLine("}");
        }

        private string getJavascriptType(string type)
        {
            switch (type)
            {
                case "byte":
                    return "number";
                case "int":
                    return "number";
                case "Int16":
                    return "number";
                case "Int32":
                    return "number";
                case "Int64":
                    return "number";
                case "UInt16":
                    return "number";
                case "UInt32":
                    return "number";
                case "UInt64":
                    return "number";
                case "float":
                    return "number";
                case "double":
                    return "number";
                case "string":
                    return "string";
                case "boolean":
                    return "boolean";
            }
            return "object";
        }

        private void OutputMethodHeader(MethodDeclaration method)
        {
            bool isPrivate = ((method.Modifiers & Modifiers.Private) == Modifiers.Private);
            if (isPrivate) { output.Add("private"); }
            output.AddWithoutSpace(method.Name);

            //example :  public void MethodA<T1>() {} 
            TypeParametersEmitter.Output(method.TypeParameters, method.Constraints, EmitterArguments);
            ParameterEmitter.Output(method.Parameters, EmitterArguments);

            output.Add(":");
            output.Add(AstTypeToStringConverter.Convert(method.ReturnType, EmitterArguments));
        }
    }
}
