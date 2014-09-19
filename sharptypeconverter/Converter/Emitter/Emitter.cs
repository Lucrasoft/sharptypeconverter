using System.Collections.Generic;
using ICSharpCode.NRefactory.CSharp;
using Converter.TypeTree;
using Converter.Emitter.OutputFunctions;

namespace Converter.Emitter
{
    internal class Emitter
    {
        private CodeSegment output;
        private readonly Tree typeTree;
        private readonly Queue<AstNode> postPonedNodes = new Queue<AstNode>();
        private readonly List<string> referencedNamespaces = new List<string>();

        public Result Result
        {
            get
            {
                return new Result{MainFile = output.Result, TypesRequested = output.TypesRequested };
            }
        }

        internal Emitter(Tree tree){
            typeTree = tree;
        }

        internal void Process(SyntaxTree syntax)
        {
            //init 
            output = new CodeSegment();
            referencedNamespaces.Clear();
            typeTree.ClearActiveType();

            //actual processing
            foreach (var node in syntax.Children)
            {
                ProcessNode(node, new EmitterArguments
                {
                    TypeTree = typeTree, 
                    Output = output, 
                    ReferencedNamespaces = referencedNamespaces, 
                    PostPonedNodes = postPonedNodes, 
                    SyntaxTree = syntax
                });
            }
           
        }

        internal static void ProcessNode(AstNode node, EmitterArguments arguments)
        {
            var nodetype = node.GetType().ToString();
            if (node.NodeType == NodeType.Expression)
            {
                // OutputExpression((Expression)node);
            }
            else
            {
                switch (nodetype)
                {
                    case "ICSharpCode.NRefactory.CSharp.Comment":
                        arguments.Output.Comment(((Comment)node).Content);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.ConstructorDeclaration":
                        new ConstructorDeclarationEmitter(arguments).Output((ConstructorDeclaration)node);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.CSharpTokenNode":
                        break;
                    case "ICSharpCode.NRefactory.CSharp.DelegateDeclaration":
                        new DelegateDeclarationEmitter(arguments).Output((DelegateDeclaration)node);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.EnumMemberDeclaration":
                        new EnumMemberDeclarationEmitter(arguments).Output((EnumMemberDeclaration)node);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.FieldDeclaration":
                        new FieldDeclarationEmitter(arguments).Output( (FieldDeclaration)node);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.Identifier":
                        break;
                    case "ICSharpCode.NRefactory.CSharp.MethodDeclaration":
                        new MethodDeclarationEmitter(arguments).Output((MethodDeclaration)node);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.NamespaceDeclaration":
                        new NamespaceDeclarationEmitter(arguments).Output((NamespaceDeclaration)node);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.OperatorDeclaration":
                        new OperatorDeclarationEmitter(arguments).Output((OperatorDeclaration)node);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.PropertyDeclaration":
                        new PropertyDeclarationEmitter(arguments).Output((PropertyDeclaration)node);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.TypeDeclaration":
                        new TypeDeclarationEmitter(arguments).Output((TypeDeclaration)node);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.UsingDeclaration":
                        new UsingDeclarationEmitter(arguments).Output((UsingDeclaration)node);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.NewLineNode":
                        arguments.Output.Add(node.ToString());
                        break;
                    case "ICSharpCode.NRefactory.CSharp.SimpleType":
                        //this type didn't exist in older csharpparser version,ToDo: find out its use.
                        break;

                    ////statements
                    ////TODO : implement -> 
                    ////case ICSharpCode.NRefactory.CSharp.CheckedStatement
                    ////case ICSharpCode.NRefactory.CSharp.DoWhileStatement
                    ////case ICSharpCode.NRefactory.CSharp.EmptyExpression
                    ////case ICSharpCode.NRefactory.CSharp.FixedStatement
                    ////case ICSharpCode.NRefactory.CSharp.FixedVariableInitializer
                    ////case ICSharpCode.NRefactory.CSharp.GotoCaseStatement
                    ////case ICSharpCode.NRefactory.CSharp.GotoDefaultStatement
                    ////case ICSharpCode.NRefactory.CSharp.GotoStatement
                    ////case ICSharpCode.NRefactory.CSharp.LabelStatement
                    ////case ICSharpCode.NRefactory.CSharp.UncheckedStatement 
                    ////case ICSharpCode.NRefactory.CSharp.YieldBreakStatement;


                    case "ICSharpCode.NRefactory.CSharp.BlockStatement":
                        new BlockStatementEmitter(arguments).Output((BlockStatement)node);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.BreakStatement":
                        arguments.Output.AddLine("break;");
                        break;
                    case "ICSharpCode.NRefactory.CSharp.ContinueStatement":
                        arguments.Output.AddLine("continue;");
                        break;
                    case "ICSharpCode.NRefactory.CSharp.ExpressionStatement":
                        new ExpressionStatementEmitter(arguments).Output((ExpressionStatement)node);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.ForStatement":
                        new ForStatementEmitter(arguments).Output((ForStatement)node);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.ForeachStatement":
                        new ForeachStatementEmitter(arguments).Output((ForeachStatement)node);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.IfElseStatement":
                        new IfElseStatementEmitter(arguments).Output((IfElseStatement)node);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.LockStatement":
                        new LockStatementEmitter(arguments).Output((LockStatement)node);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.ReturnStatement":
                        new ReturnStatementEmitter(arguments).Output((ReturnStatement)node);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.SwitchStatement":
                        new SwitchStatementEmitter(arguments).Output((SwitchStatement)node);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.ThrowStatement":
                        new ThrowStatementEmitter(arguments).Output((ThrowStatement)node);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.TryCatchStatement":
                        new TryCatchStatementEmitter(arguments).Output((TryCatchStatement)node);
                        break;
                   case "ICSharpCode.NRefactory.CSharp.UnsafeStatement":
                        new UnsafeStatementEmitter(arguments).Output((UnsafeStatement)node);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.UsingStatement":
                        new UsingStatementEmitter(arguments).Output((UsingStatement)node);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.VariableDeclarationStatement":
                        new VariableDeclarationStatementEmitter(arguments).Output((VariableDeclarationStatement)node);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.WhileStatement":
                        new WhileStatementEmitter(arguments).Output((WhileStatement)node);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.YieldReturnStatement":
                        new YieldReturnStatementEmitter(arguments).Output((YieldReturnStatement)node);
                        break;

                    //list not complete !
                    default:
                        {
                            //throw new Exception();
                            //temporary solution for fallback -> 
                            arguments.Output.Comment("TODO : review unprocessed statement");
                            arguments.Output.AddLine(node.ToString());
                            break;
                        }
                }
            }
        }
    }
}
