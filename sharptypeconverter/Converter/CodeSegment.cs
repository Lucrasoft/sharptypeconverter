using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Converter
{
    public class CodeSegment
    {
        private const string IndentString = "\t";
        private int indent;
        private bool needsIndent;
        private readonly StringBuilder mainFileBuilder;
        private readonly Dictionary<string, TypeScriptDefinitionGroup> definitionFiles;

        public string Result
        {
            get
            { return mainFileBuilder.ToString(); }
        }
        public Dictionary<string, TypeScriptDefinitionGroup> DefinitionFiles
        {
            get
            {
                return definitionFiles;
            }
        }

        public CodeSegment()
        {
            mainFileBuilder = new StringBuilder();
            definitionFiles = new Dictionary<string, TypeScriptDefinitionGroup>();
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

        public void AddTypeDefinition(string fileName, TypeScriptDefinition definition)
        {
            if (definitionFiles.ContainsKey(fileName))
            {
                definitionFiles[fileName].AddIfNew(definition);
                return;
            }
            definitionFiles.Add(fileName,
                new TypeScriptDefinitionGroup {Definitions = new List<TypeScriptDefinition> {definition}});
        }
    }
}
