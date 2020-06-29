using System;
using System.Linq;

namespace DateTagger
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("ERROR: Reference file directory and web pages directory required.");
                Console.WriteLine("Example: dotnet DateTagger.dll ./js ./html");
                return;
            }

            var argsList = args.ToList();

            bool verbose = false;
            if (argsList[0] == "-v" || argsList[0] == "-verbose")
            {
                verbose = true;
                argsList.RemoveAt(0);
            }

            string JsDirectory = argsList[0];
            string WebPagesDirectory = argsList[1];

            var fileFinder = new FileFinder();
            Console.WriteLine($"Searching {JsDirectory} for updated reference files");
            var updatedJsFiles = fileFinder.GetUpdatedJsFiles(JsDirectory);
            if (verbose)
            {
                foreach (var file in updatedJsFiles)
                {
                    Console.WriteLine($"Updated js file {file}");
                }
            }

            Console.WriteLine($"Searching {WebPagesDirectory} for references to js files");
            var webPageFiles = fileFinder.GetFilesWithJsReferences(WebPagesDirectory);
            if (verbose)
            {
                foreach (var file in webPageFiles)
                {
                    Console.WriteLine($"Web page file {file}");
                }
            }

            var tagger = new Tagger();
            var taggedFiles = tagger.TagFiles(updatedJsFiles, webPageFiles);
            if (verbose)
            {
                foreach (var file in taggedFiles)
                {
                    Console.WriteLine($"Tagged file {file}");
                }
            }
        }
    }
}