using System.Text;

namespace Converter
{
    public class CodeSegment
    {
        private const string IndentString = "\t";
        private int indent;
        private bool needsIndent;
        private readonly StringBuilder sb;

        public string Result
        {
            get
            { return sb.ToString(); }
        }

        public CodeSegment()
        {
            sb = new StringBuilder();
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
                    sb.Append(IndentString);
                }
            }
        }

        public void NewLine()
        {
            sb.AppendLine();
            needsIndent = true;
        }


        public void AddLineToTop(string text)
        {
            sb.Insert(0, text + "\r\n");
        }


        public void Add(string text)
        {
            WriteIndent();
            sb.Append(text + " ");
        }

        public void AddLine(string text)
        {
            WriteIndent();
            sb.AppendLine(text);
            needsIndent = true;
        }

        public void AddWithoutSpace(string text)
        {
            WriteIndent();
            sb.Append(text);
        }

        public void Comment(string text)
        {
            WriteIndent();
            sb.AppendLine("//" + text);
            needsIndent = true;
        }
    }
}
