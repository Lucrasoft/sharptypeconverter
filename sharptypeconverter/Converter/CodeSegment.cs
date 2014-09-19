using System.Collections.Generic;
using System;
using System.Text;

namespace Converter
{
    public class CodeSegment
    {
        private const string IndentString = "\t";
        private int indent;
        private bool needsIndent;
        private readonly StringBuilder mainFileBuilder;
        public Dictionary<string, List<Type>> TypesRequested { get; private set; }

        public string Result
        {
            get
            { return mainFileBuilder.ToString(); }
        }

        

        public CodeSegment()
        {
            mainFileBuilder = new StringBuilder();
            TypesRequested = new Dictionary<string, List<Type>>();
            indent = 0;
        }

        public void IndentIncrease()
        {
            indent++;
        }

        //decrease the line indent
        public void IndentDecrease()
        {
            indent--;
        }

        public void WriteIndent()
        {
            if (needsIndent)
            {
                needsIndent = false;
                for (int i = 0; i < indent; i++)
                {
                    mainFileBuilder.Append(IndentString);
                }
            }
        }

        public void NewLine()
        {
            mainFileBuilder.AppendLine();
            needsIndent = true;
        }


        public void AddLineToTop(string text)
        {
            mainFileBuilder.Insert(0, text + "\r\n");
        }


        public void Add(string text)
        {
            WriteIndent();
            mainFileBuilder.Append(text + " ");
        }

        public void AddLine(string text)
        {
            WriteIndent();
            mainFileBuilder.AppendLine(text);
            needsIndent = true;
        }

        public void AddWithoutSpace(string text)
        {
            WriteIndent();
            mainFileBuilder.Append(text);
        }

        public void Comment(string text)
        {
            WriteIndent();
            mainFileBuilder.AppendLine("//" + text);
            needsIndent = true;
        }

        public void AddTypeRequest(string fileName, Type definition)
        {
            if (TypesRequested.ContainsKey(fileName))
            {
                if (!TypesRequested[fileName].Contains(definition))
                {
                    TypesRequested[fileName].Add(definition);
                }
            }
            else
            {
                TypesRequested.Add(fileName, new List<Type> {definition});
            }
        }
    }
}
