﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.CSharp;

namespace Converter.Emitter
{
    internal static class ReferencedTypesEmitter
    {
        internal static Dictionary<string,string> CreateFiles(Dictionary<string, List<Type>> requestedTypes)
        {
            var newRequests = new List<Type>();
            var handledRequests = new List<Type>();
            var result = new Dictionary<string, string>();
            foreach (var file in requestedTypes)
            {
                var fileBuilder = new StringBuilder();
                foreach (var type in file.Value)
                {
                    var definitionResult = CreateTypeDefinition(type);
                    fileBuilder.AppendLine(definitionResult.Item1);
                    newRequests = newRequests.Union(definitionResult.Item2).ToList();
                    handledRequests.Add(type);
                }
                result.Add(file.Key,fileBuilder.ToString());
            }
            //before checking for already handled request convert al new request to there generic version to avoid duplicates
            newRequests = newRequests.Select(r => r.IsGenericType ? r.GetGenericTypeDefinition() : r).Except(handledRequests).ToList();
            while (newRequests.Any())
            {
                var newNewRequests = new List<Type>();
                foreach (var request in newRequests)
                {
                    var filename = request.Namespace;
                    var fileBuilder = new StringBuilder();
                    if (result.ContainsKey(filename))
                    {
                        fileBuilder.AppendLine(result[filename]);
                    }
                    else
                    {
                        result.Add(filename,"");
                    }
                    var definitionResult = CreateTypeDefinition(request);
                    fileBuilder.AppendLine(definitionResult.Item1);
                    result[filename] = fileBuilder.ToString();
                    newNewRequests = newNewRequests.Union(definitionResult.Item2).ToList(); 
                    handledRequests.Add(request);
                }
                newRequests = newNewRequests.Select(r => r.IsGenericType ? r.GetGenericTypeDefinition() : r).Except(handledRequests).ToList();
            }
            return result;
        }

        private static Tuple<string, List<Type>> CreateTypeDefinition(Type typeInfo)
        { 
            var typeName = typeInfo.Name;
            var requestedTypes = new List<Type>();
            var genericTypes = "";
            if (typeInfo.GetGenericArguments().Any())
            {
                typeName = typeInfo.Name.Substring(0, typeInfo.Name.Length - 2) + typeInfo.GetGenericArguments().Count();
                genericTypes += " <" ;
                genericTypes +=
                    typeInfo.GetGenericArguments()
                        .Select(a => a.Name)
                        .Aggregate((current, next) => current + ", " + next);
                genericTypes += ">";
            }
            var baseClass = typeInfo.BaseType;
            var modules = typeInfo.Namespace.Split('.').ToList();
            var constructors = typeInfo.GetConstructors().Select(ConvertConstructor).Where(c => c != null).ToList();
            var methods = typeInfo.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Select(ConvertMethod).Where(m => m != null).ToList();
            if (!typeInfo.IsInterface)
            {
                var explicitMethods = typeInfo.GetInterfaces()
                    .SelectMany(i => typeInfo.GetInterfaceMap(i).TargetMethods
                        .Where(m => m.IsPrivate).Select(ConvertMethod)
                        .Where(m => m != null));
                methods.AddRange(explicitMethods.Where(em => !methods.Select(m => m.Item1).Contains(em.Item1)));
            }
            var typeDeclaration = new StringBuilder("declare ");
            for (var i = 0; i < modules.Count(); i++)
            {
                typeDeclaration.AppendLine("module " + modules.ElementAt(i) + "{");
            }
            typeDeclaration.Append((typeInfo.IsInterface ? "interface " : "class ") + typeName + genericTypes);
            if (baseClass != null)
            {
                typeDeclaration.Append(" extends " + (baseClass.FullName ?? baseClass.Name));
                requestedTypes.Add(baseClass);
            }
            if (typeInfo.GetInterfaces().Any())
            {
                var interfaceResult = ConvertInterfaces(typeInfo.GetInterfaces());
                typeDeclaration.Append(" " + interfaceResult.Item1);
                requestedTypes.AddRange(interfaceResult.Item2);
            }
            typeDeclaration.AppendLine("{");
            foreach (var constructor in constructors)
            {
                typeDeclaration.AppendLine(constructor.Item1);
                requestedTypes.AddRange(constructor.Item2);
            }
            foreach (var method in methods)
            {
                typeDeclaration.AppendLine(method.Item1);
                requestedTypes.AddRange(method.Item2);
            }
            typeDeclaration.AppendLine(" }");
            for (var i = 0; i < modules.Count(); i++)
            {
                typeDeclaration.AppendLine("}");
            }
            return new Tuple<string, List<Type>>(typeDeclaration.ToString(), requestedTypes.Distinct().ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns>tuple with first item the conversion and second a list of referenced types in the constructor or
        /// null if the constructor is not supported</returns>
        private static Tuple<string, List<Type>> ConvertConstructor(ConstructorInfo info)
        {
            var builder = new StringBuilder("constructor(");
            var newRequests = new List<Type>();
            if (info.GetParameters().Any())
            {
                var convertedParameters = ConvertParameter(info.GetParameters().ToList());
                //if parameters contain unsupported types, method is not supported
                if (convertedParameters == null)
                {
                    return null;
                }
                builder.Append(convertedParameters.Item1);
                newRequests.AddRange(convertedParameters.Item2);
            }
            builder.Append(");");
            return new Tuple<string, List<Type>>(builder.ToString(), newRequests);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="interfaces"></param>
        /// <returns>tuple with first item the conversion and second a list of referenced types in the interfaces</returns>
        private static Tuple<string, List<Type>> ConvertInterfaces(Type[] interfaces)
        {
            var builder = new StringBuilder("implements ");
            var newRequests = new List<Type>();

            for(var i = 0; i< interfaces.Length; i++)
            {
                if (i > 0)
                {
                    builder.Append(", ");
                }
                if (interfaces[i].IsGenericType)
                {
                    var name = interfaces[i].Name.Substring(0, interfaces[i].Name.Length - 2) + interfaces[i].GenericTypeArguments.Count();
                    var genericTypes = interfaces[i].GenericTypeArguments.Select(t => FilterTypeForLiteral(t.FullName))
                        .Aggregate((current, next) => current + "," + next);
                    builder.Append(name + "<" + genericTypes + ">");
                }
                else
                {
                    builder.Append(interfaces[i].FullName ?? interfaces[i].Name);
                }
                newRequests.Add(interfaces[i]);
            }
            return new Tuple<string, List<Type>>(builder.ToString(), newRequests);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns>tuple with first item the conversion and second a list of referenced types in the method or
        /// null if the method is not supported</returns>
        private static Tuple<string,List<Type>> ConvertMethod(MethodInfo info)
        {
            var methodInfo = info.IsGenericMethod ? info.GetGenericMethodDefinition() : info;
            //take only last part of methodname so when it is an explicit interface implementation we get a valid name in typescript
            //(these names contain the full interface name before the methodname)
            var builder = new StringBuilder(methodInfo.Name.Split('.').Last());
            builder.Append("(");
            var newRequests = new List<Type>();
            if (methodInfo.GetParameters().Any())
            {
                var convertedParameters = ConvertParameter(methodInfo.GetParameters().ToList());
                //if parameters contain unsupported types, method is not supported
                if (convertedParameters == null)
                {
                    return null;
                }
                builder.Append(convertedParameters.Item1);
                newRequests.AddRange(convertedParameters.Item2);
            }
            var filteredType = FilterTypeForLiteral(methodInfo.ReturnType.FullName ?? methodInfo.ReturnType.Name);          
            //pointer types are not supported
            if (filteredType.Contains('*') || filteredType.Contains('&'))
            {
                return null;
            }
            if (methodInfo.ReturnType.GenericTypeArguments.Any())
            {
                var name = methodInfo.ReturnType.Name.Substring(0, methodInfo.ReturnType.Name.Length - 2) + methodInfo.ReturnType.GenericTypeArguments.Count();
                var genericTypes =
                    methodInfo.ReturnType.GenericTypeArguments.Select(t => FilterTypeForLiteral(t.FullName ?? t.Name))
                        .Aggregate((current, next) => current + "," + next);
                builder.Append(") : " + methodInfo.ReturnType.Namespace + "." + name + "<" + genericTypes + ">");
            }
            else
            {
                builder.Append(") : " + filteredType);
            }
            if (filteredType.Equals(methodInfo.ReturnType.FullName ?? methodInfo.ReturnType.Name) && !methodInfo.ReturnType.IsGenericParameter)
            {
                //request element type of array instead of array
                newRequests.Add(methodInfo.ReturnType.IsArray
                    ? methodInfo.ReturnType.GetElementType()
                    : methodInfo.ReturnType);
            }
            return new Tuple<string, List<Type>>(builder.ToString(), newRequests);
        }
        private static Tuple<string, List<Type>> ConvertParameter(List<ParameterInfo> parameters)
        {
            var resultBuilder = new StringBuilder();
            var newRequests = new List<Type>();
            for (var i = 0; i<parameters.Count(); i++)
            {
                var param = parameters[i];
                if (i > 0)
                {
                    resultBuilder.Append(", ");
                }
                var type = FilterTypeForLiteral(param.ParameterType.FullName?? param.ParameterType.Name);
                
                //pointer types are not supported
                if (type.Contains('*') || type.Contains('&'))
                {
                    return null;
                }
                if (param.ParameterType.GenericTypeArguments.Any())
                {
                    var name = param.ParameterType.Name.Substring(0, param.ParameterType.Name.Length - 2) + param.ParameterType.GenericTypeArguments.Count();
                    var genericTypes =
                        param.ParameterType.GenericTypeArguments.Select(t => FilterTypeForLiteral(t.FullName))
                            .Aggregate((current, next) => current + "," + next);
                    resultBuilder.Append(param.Name + " : " + param.ParameterType.Namespace + "." + name + "<" + genericTypes + ">");
                }
                else
                {
                    resultBuilder.Append(param.Name + " : " + type);
                }
                if (type.Equals(param.ParameterType.FullName))
                {
                    //request element type of array instead of array
                    if (param.ParameterType.IsArray)
                    {
                        newRequests.Add(param.ParameterType.GetElementType());
                    }
                    else
                    {
                        newRequests.Add(param.ParameterType);
                    }
                }
            }
            return new Tuple<string, List<Type>>(resultBuilder.ToString(), newRequests);
        }
        private static string FilterTypeForLiteral(string type)
        {
            switch (type)
            {
                case "System.String":
                    return "string";
                case "System.Boolean":
                    return "boolean";
                case "System.Int32":
                    return "number";
                case "System.Double":
                    return "number";
                case "System.Float":
                    return "number";
                case "System.Void":
                    return "void";
                case "System.String[]":
                    return "string[]";
                case "System.Boolean[]":
                    return "boolean[]";
                case "System.Int32[]":
                    return "number[]";
                default:
                    return type;
            }
        }
    }
}
