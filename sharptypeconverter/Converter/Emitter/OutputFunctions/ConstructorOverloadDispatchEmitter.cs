using System;
using System.Collections.Generic;
using System.Linq;
using Converter.Emitter.OutputFunctions.HelperFunctions;
using Converter.TypeTree;
using ICSharpCode.NRefactory.CSharp;

namespace Converter.Emitter.OutputFunctions
{
    internal static class ConstructorOverloadDispatchEmitter
    {
        internal static void Output(TIConstructor constructor, EmitterArguments arguments)
        {
            var output = arguments.Output;
            var syntaxTree = arguments.SyntaxTree;
            var tempd = constructor.declarations[0];
            var refConstructor = syntaxTree.GetNodeAt<ConstructorDeclaration>(tempd.Line, tempd.Column);
            output.Comment("Overloaded constructor dispatch function");
            output.Add("constructor(...params : any[]) {");
            output.NewLine();
            output.IndentIncrease();
            output.AddLine("this.constructorDispatcher(params);");
            output.Comment("surpress compiler warning about missing super");
            output.AddLine("if (!true) { super(); }");
            output.IndentDecrease();
            output.AddLine("}");
            output.NewLine();
            output.Add("constructorDispatcher(...params : any[]) {");
            output.NewLine();
            output.IndentIncrease();
            //first attempt
            //var c = 0;
            //check all declarations for the number of required parameters, and group them per number of parameters;
            var declGroup = new Dictionary<Int32, List<ConstructorDeclaration>>();
            foreach (var d in constructor.declarations)
            {
                var item = syntaxTree.GetNodeAt<ConstructorDeclaration>(d.Line, d.Column);
                var nrOfParams = item.Parameters.Count;
                if (!declGroup.ContainsKey(nrOfParams))
                {
                    declGroup.Add(nrOfParams, new List<ConstructorDeclaration>());
                }
                declGroup[nrOfParams].Add(item);
            }

            foreach (var group in declGroup.Keys)
            {
                //per nrOfParameters check how many declarations are there. 
                //if only 1 declaration, then dispatcher only needs to check the number of parameters to get a good match.
                var declarations = declGroup[group];
                if (declarations.Count == 1)
                {
                    var item = declarations[0];
                    var index = (from d in constructor.declarations where (d.Line == item.StartLocation.Line) & (d.Column == item.StartLocation.Column) select d.Index).FirstOrDefault();
                    output.AddLine("if (params.length==" + group + ") {");
                    output.IndentIncrease();
                    //make the call to actual constructor implementation
                    output.Add(string.Format("this.{0}__{1}(", refConstructor.Name, index));
                    for (int i = 0; i < group; i++)
                    {
                        output.Add("params[" + i + "]");
                        if (i < item.Parameters.Count - 1)
                        {
                            output.Add(",");
                        }
                    }
                    output.Add(");");
                    output.NewLine();
                    output.AddLine("return;");
                    output.IndentDecrease();
                    output.AddLine("}");
                }
                else
                {
                    //more then one declaration for this number of Parameters
                    //find the right overload based on typeof paramaters. (not perfect!)
                    output.AddLine("if (params.length==" + group + ") {");
                    output.IndentIncrease();
                    foreach (var item in declarations)
                    {
                        var index = (from d in constructor.declarations where (d.Line == item.StartLocation.Line) & (d.Column == item.StartLocation.Column) select d.Index).FirstOrDefault();

                        output.AddWithoutSpace(" if (");
                        for (int i = 0; i < group; i++)
                        {
                            output.Add("($TS.TypeEqualsString(params[" + i + "],'" + AstTypeToStringConverter.Convert(item.Parameters.ElementAt(i).Type, arguments) + "'))");
                            if (i < group - 1) { output.AddLine(" && "); }
                        }
                        output.AddLine(") {");
                        output.IndentIncrease();
                        //make the call to actual constructor implementation
                        output.Add(string.Format("this.{0}__{1}(", refConstructor.Name, index));
                        for (int i = 0; i < group; i++)
                        {
                            output.Add("params[" + i + "]");
                            if (i < item.Parameters.Count - 1)
                            {
                                output.Add(",");
                            }
                        }
                        output.Add(");");
                        output.NewLine();
                        output.AddLine("return;");
                        output.IndentDecrease();
                        output.AddLine("}");
                    }
                    output.IndentDecrease();
                    output.AddLine("}");
                }
            }
            output.IndentDecrease();
            output.AddLine("}");
        }
    }
}
