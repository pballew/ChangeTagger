using System;
using System.Collections.Generic;
using System.IO;

namespace DateTagger
{
    public class FileFinder
    {
        public List<string> WebPageFileExtensions { get; set; } = new List<string>() { "aspx", "ascx", "html", "master", "cshtml" };
        public List<string> ReferencedFileExtensions { get; set; } = new List<string>() { "js", "css" };

        public List<string> GetFilesWithJsReferences(string path)
        {
            var files = new List<string>();
            foreach (string extension in WebPageFileExtensions)
            {
                files.AddRange(Directory.GetFiles(path, $"*.{extension}"));
            }

            var dirs = Directory.EnumerateDirectories(path);
            foreach (var dir in dirs)
            {
                files.AddRange(GetFilesWithJsReferences(dir));
            }
            return files;
        }

        public List<string> GetUpdatedJsFiles(string path)
        {
            var newFiles = new List<string>();

            foreach (var extension in ReferencedFileExtensions)
            {
                var allFiles = Directory.GetFiles(path, $"*.{extension}");
                foreach (var file in allFiles)
                {
                    var lastWriteTime = File.GetLastWriteTime(file);
                    if (DateTime.Now.AddHours(-12) < lastWriteTime)
                    {
                        newFiles.Add(file);
                    }
                }
            }

            var dirs = Directory.EnumerateDirectories(path);
            foreach (var dir in dirs)
            {
                newFiles.AddRange(GetUpdatedJsFiles(dir));
            }

            return newFiles;
        }
    }
}