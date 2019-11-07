using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncQuickInfo
{
    public class LinesCounter
    {
        private bool IsOpeningLine(string l, int br)
        {
            if (br < -1)
            {
                return false;
            }
            if (!l.Contains("{"))
            {
                return false;
            }
            if (l.Contains("{") && l.Contains("}"))
            {
                var countOpen = l.ToCharArray().Where(x => x == '{').Count();
                var countClosed = l.ToCharArray().Where(x => x == '}').Count();
                if (countOpen > countClosed)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private int CountCodeLinesBetween(string[] lines, int start, int end)
        {
            int codeLines = 0;
            for (int i = start + 1; i < end; i++)
            {
                var l = lines[i].Trim();
                while (l.Contains(" "))
                {
                    l = l.Replace(" ", "");
                }
                if (string.IsNullOrEmpty(l))
                {
                    continue;
                }
                if (l.StartsWith("//"))
                {
                    continue;
                }
                if (l == "{" || l.StartsWith("{//"))
                {
                    continue;
                }
                if (l == "}" || l.StartsWith("}//"))
                {
                    continue;
                }
                if (l.StartsWith("#"))
                {
                    continue;
                }
                codeLines++;
            }
            return codeLines;
        }

        public int GetCurrentLinesInMethod(string[] lines, int currentLine)
        {
            var startScan = currentLine;
            var brackets = -1;
            while (startScan > 0 && !IsOpeningLine(lines[startScan], brackets) && brackets < 0)
            {
                string l = lines[startScan];
                var countOpen = l.ToCharArray().Where(x => x == '{').Count();
                var countClosed = l.ToCharArray().Where(x => x == '}').Count();
                brackets += countOpen - countClosed;
                startScan--;
            }
            if (startScan == 0)
            {
                return lines.Length;
            }
            int openBrackets = 1;
            int endScan = startScan + 1;
            while (openBrackets > 0 && endScan < lines.Length)
            {

                string l = lines[endScan];
                var countOpen = l.ToCharArray().Where(x => x == '{').Count();
                var countClosed = l.ToCharArray().Where(x => x == '}').Count();
                openBrackets += countOpen - countClosed;
                endScan++;
            }
            if (endScan == lines.Length)
            {
                return lines.Length;
            }
            endScan--;
            return CountCodeLinesBetween(lines, startScan, endScan);
        }
    }
}
