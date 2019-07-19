using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ChangeTagger
{
    public class Tagger
    {
        public List<string> TagFiles(List<string> jsFiles, List<string> webPageFiles)
        {
            var taggedFiles = new List<string>();

            foreach (var file in webPageFiles)
            {
                if (TagFile(file, jsFiles))
                {
                    taggedFiles.Add(file);
                }
            }

            return taggedFiles;
        }

        public bool TagFile(string webPageFile, List<string> jsFiles)
        {
            bool wasTagged = false;

            var lines = new List<string>(File.ReadLines(webPageFile));
            for (int i = 0; i < lines.Count; i++)
            {
                foreach (string jsFile in jsFiles)
                {
                    var filename = Path.GetFileName(jsFile);
                    if (lines[i].Contains(filename))
                    {
                        string pattern = $"/{filename}[\\?\\d+]*";
                        string tag = DateTime.Now.ToString("MMddyyyy");
                        string replacementText = $"/{filename}?{tag}";
                        lines[i] = Regex.Replace(lines[i], pattern, replacementText);
                        wasTagged = true;
                    }
                }
            }

            if (wasTagged)
            {
                File.Delete(webPageFile);
                File.WriteAllLines(webPageFile, lines, new UTF8Encoding(true));
            }

            return wasTagged;
        }
    }
}