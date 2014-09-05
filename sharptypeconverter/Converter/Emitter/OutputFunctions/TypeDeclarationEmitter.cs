using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.CSharp;
using Converter.TypeTree;

namespace Converter.Emitter.OutputFunctions
{
    internal class TypeDeclarationEmitter : BaseEmitter<TypeDeclaration>
    {
        readonly Tree typeTree;
        readonly Queue<AstNode> postPonedNodes;
        readonly CodeSegment output;
        internal TypeDeclarationEmitter(EmitterArguments arguments)
            : base(arguments)
        {
            typeTree = arguments.TypeTree;
            postPonedNodes = arguments.PostPonedNodes;
            output = arguments.Output;
        }
        internal override void Output(TypeDeclaration node)
        {

            if (typeTree.ActiveType != null)
            {
                // a second type declaration within a activetype declaration is not support by typescript
                // postpone the proces of this type 
                postPonedNodes.Enqueue(node);
                return;
            }
            typeTree.SetActiveType(node.Name, node.ClassType);

            if (string.IsNullOrEmpty(typeTree.ActiveNameSpace)) 
            {
                typeTree.PushNameSpace(typeTree.DefaultNamespace.Namespace);
                output.Add(string.Format("module {0} ", typeTree.DefaultNamespace.Namespace));
                output.AddLine("{");
                output.IndentIncrease();
            }

            switch (node.ClassType)
            {
                case ClassType.Class:
                    ClassAndStructDeclarationEmitter.Output(node,EmitterArguments);
                    break;
                case ClassType.Enum:
                    EnumTypeDeclarationEmitter.Output(node,EmitterArguments);
                    break;
                case ClassType.Interface:
                    TypeDeclarationInterfaceEmitter.Output(node,EmitterArguments);
                    break;
                case ClassType.Struct:
                    ClassAndStructDeclarationEmitter.Output(node, EmitterArguments);
                    break;
                default:
                    throw new Exception("This type declaration is not handled");
            }
            if (typeTree.ActiveNameSpace.Equals(typeTree.DefaultNamespace.Namespace))
            {
                output.IndentDecrease();
                output.AddLine("");
                output.AddLine("}");
                typeTree.PopNameSpace();
            }

            typeTree.ClearActiveType();
            while (postPonedNodes.Count > 0)
            {
                var postnode = postPonedNodes.Dequeue();
                Emitter.ProcessNode(postnode, EmitterArguments);
            }
        }
    }
}
