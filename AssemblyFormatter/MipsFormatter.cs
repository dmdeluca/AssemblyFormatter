using System.Linq;
using System.Text.RegularExpressions;

namespace AssemblyFormatter
{
    public class MipsFormatter : IMipsFormatter
    {
        public string Format(string fullFileText)
        {
            fullFileText = Regex.Replace(fullFileText, @",\s+", ", ");
            fullFileText = Regex.Replace(fullFileText, @"\n\n+", "\n");
            fullFileText = Regex.Replace(fullFileText, @"\s+#\s+", "  #  ");
            fullFileText = Regex.Replace(fullFileText, @":\s+#", ":\n #");
            fullFileText = Regex.Replace(fullFileText, @"\n\s+\n", "\n");
            fullFileText = Regex.Replace(fullFileText, @"\t\$", " $");
            fullFileText = Regex.Replace(fullFileText, @"\s\s+\$", " $");
            fullFileText = Regex.Replace(fullFileText, @"\n\s*\.", "\n\t.");
            fullFileText = Regex.Replace(fullFileText, @"\s+0x", "    0x");
            fullFileText = Regex.Replace(fullFileText, @",\s+0x", ", 0x");
            fullFileText = Regex.Replace(fullFileText, @"\n\t+", "\n    ");
            fullFileText = Regex.Replace(fullFileText, @"\n\s*(?=\w*:)", "\n");
            fullFileText = Regex.Replace(fullFileText, @"\n\s*(?=.\w*\s)", "\n\t");
            fullFileText = EvenlySpaceComments(fullFileText);
            return fullFileText;
        }

        private static string EvenlySpaceComments(string inputText)
        {
            var evenlySpacedComments = "";
            var maxCodeLength = GetMaxCodeLength(inputText);

            foreach (var line in inputText.Split('\n'))
            {
                var fixedLine = Regex.Replace(line, @"\s+#\s+", "~");
                var split = fixedLine.Split('~');
                if (split.Length > 1)
                {
                    var calculatedSpace = new string(Enumerable.Repeat(' ', maxCodeLength - split[0].Length).ToArray());
                    evenlySpacedComments += $"{split[0]}{calculatedSpace}# {split[1]}\n";
                }
                else
                {
                    evenlySpacedComments += split[0] + "\n";
                }
            };

            return evenlySpacedComments;
        }

        private static int GetMaxCodeLength(string inputText)
        {
            return inputText.Split('\n')
                .Select(s => new string(s.TakeWhile(t => t != '#').ToArray()))
                .Max(l => l.Length);
        }

    }
}
