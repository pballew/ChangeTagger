using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;

namespace ChangeTagger
{
    class Program
    {
        public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [Argument(0, Description = "A directory containing javascript files must be specified.")]
        [Required]
        public string JsDirectory { get; }

        [Argument(1, Description = "A directory containing web pages must be specified.")]
        [Required]
        public string WebPagesDirectory { get; }

        private int OnExecute()
        {
            try
            {
                var fileFinder = new FileFinder();

                Console.WriteLine($"Searching {JsDirectory} for updated js files");
                var updatedJsFiles = fileFinder.GetUpdatedJsFiles(JsDirectory);
                //foreach (var file in updatedJsFiles)
                //{
                //    Console.WriteLine($"Updated js file {file}");
                //}

                Console.WriteLine($"Searching {WebPagesDirectory} for references to js files");
                var webPageFiles = fileFinder.GetFilesWithJsReferences(WebPagesDirectory);
                //foreach (var file in webPageFiles)
                //{
                //    Console.WriteLine($"Web page file {file}");
                //}

                var tagger = new Tagger();
                var taggedFiles = tagger.TagFiles(updatedJsFiles, webPageFiles);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }
    }
}