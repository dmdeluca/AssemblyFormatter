using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AssemblyFormatter
{
    public class Program
    {
        private static void Main(string[] args)
        {
            string fileName = GetFileName(args);
            string inputText = GetInputText(fileName);
            string formattedMips = FormatMips(inputText);
            string outputFileName = Directory.GetCurrentDirectory() + $"\\formatted_{Path.GetFileName(fileName)}";
            WriteOutput(outputFileName, formattedMips);
            Console.Write(formattedMips);
        }

        private static string GetFileName(string[] args)
        {
            string fileName;
            if (args.Length < 1)
                fileName = "hw2.asm";
            else
                fileName = args[0];
            return fileName;
        }

        private static void WriteOutput(string fileName, string outputText)
        {
            File.WriteAllText(fileName, outputText, Encoding.ASCII);
            return;
        }

        private static string FormatMips(string inputText)
        {
            inputText = Regex.Replace(inputText, @",\s+", ", ");
            inputText = Regex.Replace(inputText, @"\n\n+", "\n");
            inputText = Regex.Replace(inputText, @"\s+#\s+", "  #  ");
            inputText = Regex.Replace(inputText, @":\s+#", ":\n #");
            inputText = Regex.Replace(inputText, @"\n\s+\n", "\n");
            inputText = Regex.Replace(inputText, @"\t\$", " $");
            inputText = Regex.Replace(inputText, @"\s\s+\$", " $");
            inputText = Regex.Replace(inputText, @"\n\s*\.", "\n\t.");
            inputText = Regex.Replace(inputText, @"\s+0x", "    0x");
            inputText = Regex.Replace(inputText, @",\s+0x", ", 0x");
            inputText = Regex.Replace(inputText, @"\n\t+", "\n    ");
            inputText = Regex.Replace(inputText, @"\n\s*(?=\w*:)", "\n");
            inputText = Regex.Replace(inputText, @"\n\s*(?=.\w*\s)", "\n\t");
            inputText = EvenlySpaceComments(inputText);
            return inputText;
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

        private static string GetInputText(string fileName)
        {
            if (!File.Exists(fileName))
                return "no file";
            var fileText = File.ReadAllText(fileName, Encoding.ASCII);
            return fileText;
        }
    }
}