using System;
using System.IO;
using System.Text;

namespace AssemblyFormatter
{
    public class Application : IApplication
    {
        private readonly IMipsFormatter _mipsFormatter;

        public Application(IMipsFormatter mipsFormatter)
        {
            _mipsFormatter = mipsFormatter;
        }

        public void Run(string[] args)
        {
            string fileName = GetFileName(args);
            string inputText = GetInputText(fileName);

            string formattedMips = _mipsFormatter.Format(inputText);

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

        private static string GetInputText(string fileName)
        {
            if (!File.Exists(fileName))
                return "no file";
            var fileText = File.ReadAllText(fileName, Encoding.ASCII);
            return fileText;
        }
    }
}
