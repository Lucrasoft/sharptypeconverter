using ICSharpCode.NRefactory.CSharp;
using System;
using System.Linq;

namespace Converter.Emitter.OutputFunctions.HelperFunctions
{
    static class AstTypeToStringConverter
    {
        public static string Convert(AstType node, EmitterArguments arguments)
        {
            var typeTree = arguments.TypeTree;
            var namespaces = arguments.ReferencedNamespaces;
            if (node is SimpleType)
            {
                var st = (SimpleType)node;
                var result = st.Identifier;

                //probeer Identifier te vinden in de typeTree om tot een directe fully namespace aanroep te komen
                //zoek eerst in de huidige namespace

                if (typeTree.ExistsTypeInNamespace(result, typeTree.ActiveNameSpace))
                {
                    result = typeTree.ActiveNameSpace + "." + result;
                }
                else if (typeTree.DefaultNamespace.ExistsType(result))
                {
                    result = typeTree.DefaultNamespace.Namespace + "." + result;
                }
                else
                {
                    var nameSpace = namespaces.FirstOrDefault(ns => typeTree.ExistsTypeInNamespace(result, ns));
                    if (string.IsNullOrEmpty(nameSpace))
                    {
                        result = TypeDeclarationExtractor.ExtractFromNamespaces(result,arguments);
                    }
                    else
                    {
                         result = nameSpace + "." + result;
                    }
                }
                result += TypeArgumentsToString(st.TypeArguments, arguments);
                return result;
            }
             if (node is PrimitiveType)
            {
                var pt = (PrimitiveType)node;
                switch (pt.KnownTypeCode)
                {
                    //TODO : add more defaults
                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.Array:
                    //    //TODO !
                    //    break;
                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.Attribute:
                    //    break;
                    case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.Boolean:
                        return "boolean";

                    case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.Byte:
                        return "number";

                    case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.Char:
                        return "string";

                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.DBNull:
                    //    break;
                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.DateTime:
                    //    break;
                    case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.Decimal:
                        return "number";

                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.Delegate:
                    //    break;
                    case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.Double:
                        return "number";

                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.Enum:
                    //    break;
                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.Exception:
                    //    break;
                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.ICollection:
                    //    break;
                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.ICollectionOfT:
                    //    break;
                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.ICriticalNotifyCompletion:
                    //    break;
                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.IDisposable:
                    //    break;
                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.IEnumerable:
                    //    break;
                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.IEnumerableOfT:
                    //    break;
                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.IEnumerator:
                    //    break;
                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.IEnumeratorOfT:
                    //    break;
                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.IList:
                    //    break;
                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.IListOfT:
                    //    break;
                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.INotifyCompletion:
                    //    break;
                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.IReadOnlyListOfT:
                    //    break;
                    case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.Int16:
                        return "number";

                    case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.Int32:
                        return "number";

                    case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.Int64:
                        return "number";

                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.IntPtr:
                    //    break;
                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.MulticastDelegate:
                    //    break;
                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.None:
                    //    break;
                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.NullableOfT:
                    //    break;
                    case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.Object:
                        return "any";
                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.SByte:
                    //    break;
                    case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.Single:
                        return "number";

                    case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.String:
                        return "string";

                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.Task:
                    //    break;
                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.TaskOfT:
                    //    break;
                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.Type:
                    //    break;
                    case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.UInt16:
                        return "number";

                    case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.UInt32:
                        return "number";

                    case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.UInt64:
                        return "number";

                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.UIntPtr:
                    //    break;
                    //case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.ValueType:
                    //    break;
                    case ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.Void:
                        return "void";

                    default:
                        return "TODO-" + pt.KnownTypeCode;

                }
                //return "";
                //pt.KnownTypeCode = ICSharpCode.NRefactory.TypeSystem.KnownTypeCode.
            }
            if (node is ComposedType)
            {
                var ct = (ComposedType)node;
                string result;
                // array's
                // TS doesn't support multidimensional array's like  int[,] 
                // But does support array like int[][]
                // Either case , we generate an array with [][].
                var totalBrackets = 0;
                foreach (var item in ct.ArraySpecifiers)
                {
                    totalBrackets += item.Dimensions;
                }

                result = Convert(ct.BaseType, arguments);
                for (int i = 0; i < totalBrackets; i++)
                {
                    result += "[]";
                }
                return result;

                //throw new NotImplementedException();
            }
            if (node is MemberType)
            {
                var mt = (MemberType)node;
                //type args ?
                return mt.Target + "." + mt.MemberName + TypeArgumentsToString(mt.TypeArguments, arguments);

            }
            throw new NotImplementedException();
        }
        private static string TypeArgumentsToString(AstNodeCollection<AstType> typeArguments, EmitterArguments arguments)
        {
            //Example : List<string> 
            if (typeArguments.Count == 0) return "";
            string result = "";
            result += "<" + Convert(typeArguments.ElementAt(0), arguments);
            foreach (AstType arg in typeArguments.Skip(1))
            {
                result += ", " + Convert(arg, arguments);
            }
            result += ">";
            return result;
        }
    }
}
