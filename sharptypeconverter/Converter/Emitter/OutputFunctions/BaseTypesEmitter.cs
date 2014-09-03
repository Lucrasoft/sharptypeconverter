using Converter.Emitter.OutputFunctions.HelperFunctions;
using ICSharpCode.NRefactory.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Converter.Emitter.OutputFunctions
{
    internal static class BaseTypesEmitter 
    {
        internal static void Output(AstNodeCollection<AstType> baseTypes, bool isClass, EmitterArguments arguments)
        {
            var output = arguments.Output;
            //TODO : how to detect per BaseType if it is a class or a interface ?
            // when a builtin
            // currently convention based implemenation : if it starts with an I , and second character is also Capital, assume that it is an interface and not a class;
            var baseInterfaces = new List<string>();
            var baseClasses = new List<string>();

            for (int i = 0; i < baseTypes.Count; i++)
            {
                var item = baseTypes.ElementAt(i);
                var shortName = "";

                //AstTypeToString(item);
                if (item.GetType().Name == "MemberType")
                {
                    shortName = ((MemberType)item).MemberName;
                }
                if (item.GetType().Name == "SimpleType")
                {
                    shortName = (item).ToString();
                }
                if (shortName.Length == 0) { throw new NotImplementedException(); }

                //only the first baseType could be a Class, otherwise always an interface
                if (baseClasses.Count == 0)
                {
                    //check if name starts with I and the character is Uppercase -> if so, assume an interface.
                    if ((shortName.StartsWith("I")) && (shortName.Length > 1) && (char.IsUpper(shortName[1])))
                    {
                        baseInterfaces.Add(AstTypeToStringConverter.Convert(item,arguments));
                    }
                    else
                    {
                        baseClasses.Add(AstTypeToStringConverter.Convert(item, arguments));
                    }
                }
                else
                {
                    baseInterfaces.Add(AstTypeToStringConverter.Convert(item, arguments));
                }

            }

            if (isClass)
            {
                //add baseclass
                if (baseClasses.Count > 0)
                {
                    output.Add("extends");
                    output.Add(baseClasses[0]);
                }
                //add interface implementations
                if (baseInterfaces.Count > 0)
                {
                    output.Add("implements");
                    for (int i = 0; i < baseInterfaces.Count; i++)
                    {
                        output.Add(baseInterfaces[i]);
                        if (i < baseInterfaces.Count - 1)
                        {
                            output.Add(",");
                        }
                    }
                }
            }
            else // it is an interface we are dealing with
            {
                if (baseInterfaces.Count > 0)
                {
                    //in case of the parent is an interface we must extend the found baseInterfaces , not implement it
                    output.Add("extends");
                    for (int i = 0; i < baseInterfaces.Count; i++)
                    {
                        output.Add(baseInterfaces[i]);
                        if (i < baseInterfaces.Count - 1)
                        {
                            output.Add(",");
                        }
                    }
                }
            }
        }
    }
}
