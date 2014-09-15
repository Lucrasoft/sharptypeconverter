using Converter.Emitter.OutputFunctions.HelperFunctions;
using Converter.Emitter.OutputFunctions.RootEmitters;
using ICSharpCode.NRefactory.CSharp;
using System;
using System.Linq;

namespace Converter.Emitter.OutputFunctions
{
    static class ExpressionEmitter
    {
        /// <summary>
        /// Evaluates given expression and adds the corresponding typescript code to the output
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="arguments"></param>
        internal static void Output(Expression expression, EmitterArguments arguments){
            var output = arguments.Output;
            switch (expression.GetType().Name)
            {
                case "AnonymousMethodExpression":
                    var ame = (AnonymousMethodExpression)expression;
                    ParameterEmitter.Output(ame.Parameters, arguments);
                    output.Add("=>");
                    Emitter.ProcessNode(ame.Body,arguments);
                    break;
                case "AsExpression":
                    var ase = (AsExpression)expression;
                    output.AddWithoutSpace("Statements.As(");
                    Output(ase.Expression,arguments);
                    output.AddWithoutSpace(",");
                    output.AddWithoutSpace(AstTypeToStringConverter.Convert(ase.Type,arguments));
                    output.AddWithoutSpace(")");
                    break;
                case "AssignmentExpression":
                    var ae = (AssignmentExpression)expression;
                    Output(ae.Left,arguments);
                    AssignmentOperatorEmitter.Output(ae.Operator,output);
                    Output(ae.Right,arguments);
                    break;
                case "BaseReferenceExpression":
                    var bre = (BaseReferenceExpression)expression;
                    output.AddWithoutSpace("super");
                    break;
                case "BinaryOperatorExpression":
                    var binopExp = (BinaryOperatorExpression)expression;
                    Output(binopExp.Left,arguments);
                    BinaryOperatorEmitter.Output(binopExp.Operator,output);
                    Output(binopExp.Right,arguments);
                    break;
                //example : var a= (ISomething)someThingElse;
                case "CastExpression":
                    var castExpr = (CastExpression)expression;
                    output.AddWithoutSpace("<");
                    output.AddWithoutSpace(AstTypeToStringConverter.Convert(castExpr.Type,arguments));
                    output.AddWithoutSpace(">");
                    Output(castExpr.Expression,arguments);
                    break;
                case "ConditionalExpression":
                    var ce = (ConditionalExpression)expression;
                    Output(ce.Condition, arguments);
                    output.Add(" ?");
                    Output(ce.TrueExpression,arguments);
                    output.Add(" :");
                    Output(ce.FalseExpression,arguments);
                    break;
                case "DirectionExpression":
                    //example : {ref someThing} 
                    //example : {out someThing}
                    var de = (DirectionExpression)expression;
                    //TODO : skip ref/out ..
                    Output(de.Expression,arguments);
                    break;
                case "IdentifierExpression":
                    //check if Identifier is a locally known identifier (which needs "this." prefix).
                    string identStr = expression.ToString();

                    if (arguments.TypeTree.ActiveType.NameExists(identStr))
                    {
                        output.AddWithoutSpace("this.");
                    }
                    output.AddWithoutSpace(identStr);
                    break;
                case "InvocationExpression":
                    var ie = (InvocationExpression)expression;
                    Output(ie.Target,arguments);
                    output.Add("(");
                    for (int i = 0; i < ie.Arguments.Count; i++)
                    {
                        Output(ie.Arguments.ElementAt(i),arguments);
                        if (i < ie.Arguments.Count - 1) { output.Add(","); }
                    }
                    output.Add(")");
                    break;
                case "IndexerExpression":
                    var indexExpr = (IndexerExpression)expression;
                    Output(indexExpr.Target,arguments);
                    output.AddWithoutSpace("[");
                    new ArgumentsEmitter(arguments).Output(indexExpr.Arguments);
                    output.AddWithoutSpace("]");
                    break;
                case "IsExpression":
                    var isExpr = (IsExpression)expression;
                    output.AddWithoutSpace("System.Statements.is(");
                    Output(isExpr.Expression,arguments);
                    output.AddWithoutSpace(@",""");
                    output.AddWithoutSpace(AstTypeToStringConverter.Convert(isExpr.Type,arguments));
                    output.AddWithoutSpace(@""")");
                    break;
                case "MemberReferenceExpression":
                    var mre = (MemberReferenceExpression)expression;
                    Output(mre.Target,arguments);
                    output.AddWithoutSpace(".");
                    output.AddWithoutSpace(mre.MemberName);
                    break;
                case "NullExpression":
                    output.Add("null");
                    break;
                case "NullReferenceExpression":
                    output.Add("null");
                    break;
                case "ObjectCreateExpression":
                    var oce = (ObjectCreateExpression)expression;
                    //TODO : find out if we need to use the static-Create on internal objects or the New ;
                    string objectNameSpace;
                    if (arguments.TypeTree.DefaultNamespace.ExistsType(oce.Type.ToString()))
                    {
                        objectNameSpace = arguments.TypeTree.DefaultNamespace.Namespace;
                    }
                    else
                    {
                        objectNameSpace =
                            arguments.ReferencedNamespaces.FirstOrDefault(
                                ns => arguments.TypeTree.ExistsTypeInNamespace(oce.Type.ToString(), ns));
                    }
                    if (objectNameSpace != null)
                    {
                        output.Add("new " + objectNameSpace + "." + oce.Type + "()");
                    }
                    else
                    {
                        var type = TypeDeclarationExtractor.ExtractFromNamespaces(oce.Type.ToString(), arguments);
                        output.Add("new " + type + "()");
                    }
                    break;
                case "ParenthesizedExpression":
                    var pe = (ParenthesizedExpression)expression;
                    output.AddWithoutSpace("(");
                    Output(pe.Expression,arguments);
                    output.AddWithoutSpace(")");
                    break;
                case "PrimitiveExpression":
                    PrimitiveValueEmitter.Output(((PrimitiveExpression)expression).Value,output);
                    break;
                case "TypeOfExpression":
                    var toe = (TypeOfExpression)expression;
                    output.AddWithoutSpace("System.Statements.typeOf(");
                    output.AddWithoutSpace(AstTypeToStringConverter.Convert(toe.Type,arguments));
                    output.AddWithoutSpace(")");
                    break;
                case "TypeReferenceExpression":
                    //TODO : better 
                    //FIND type
                    string typeNameSpace;
                    if (arguments.TypeTree.DefaultNamespace.ExistsType(expression.ToString()))
                    {
                        typeNameSpace = arguments.TypeTree.DefaultNamespace.Namespace;
                    }
                    else
                    {
                        typeNameSpace =
                            arguments.ReferencedNamespaces.FirstOrDefault(
                                ns => arguments.TypeTree.ExistsTypeInNamespace(expression.ToString(), ns));
                    }
                    if (typeNameSpace != null)
                    {
                        output.AddWithoutSpace(typeNameSpace + "." + expression);
                    }
                    else
                    {
                        output.AddWithoutSpace(expression.ToString());
                    }
                    break;
                case "ThisReferenceExpression":
                    output.AddWithoutSpace("this");
                    break;
                case "UnaryOperatorExpression":
                    var uoe = (UnaryOperatorExpression)expression;
                    Output(uoe.Expression,arguments);
                    UnaryOperatorEmitter.Output(uoe.Operator,output);
                    break;
                //array stuff
                case "ArrayInitializerExpression":
                    //example : int[] x = {1,2,3,4};
                    var aie = (ArrayInitializerExpression)expression;
                    output.AddWithoutSpace("[");
                    Output(aie.Elements.ElementAt(0),arguments);
                    foreach (var item in aie.Elements.Skip(1))
                    {
                        output.AddWithoutSpace(",");
                        Output(item,arguments);
                    }
                    output.AddWithoutSpace("]");
                    break;
                case "CheckedExpression":
                    var che = (CheckedExpression)expression;
                    Output(che.Expression,arguments);
                    break;
                case "ArrayCreateExpression":
                    var ace = (ArrayCreateExpression)expression;
                    //when initializer is given forget creating an array and only use initialiser;
                    if (!ace.Initializer.IsNull)
                    {
                        Output(ace.Initializer,arguments);
                    }
                    else
                    // given is : int[] x = new int[3];
                    // problem with multidimensional array's is , that js knows a really bad new Array(??) .. thats ambigous
                    // Read  http://bonsaiden.github.io/JavaScript-Garden/#arrayctor
                    {
                        if (ace.Arguments.Count > 1)
                        {
                            //ToDo find uit hoew to initialize : var x = new int[3,4]; in typescript/js.
                            output.AddWithoutSpace("$TS.createArray(");
                            for (int i = 0; i < ace.Arguments.Count; i++)
                            {
                                Output(ace.Arguments.ElementAt(i),arguments);
                                if (i < ace.Arguments.Count - 1) { output.Add(","); }
                            }
                        }
                        else
                        {
                            output.AddWithoutSpace("new Array(");
                            Output(ace.Arguments.ElementAt(0),arguments);
                            output.AddWithoutSpace(")");
                        }
                    }
                    break;
                case "DefaultValueExpression":
                    //default operator not supported in typescript since no type information is available at runtime
                    //since javascript handles null as false in boolean expressions and as 0 in math expressions it is less critical in javascript.
                    output.Add("null");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
