using System;
using System.Collections.Generic;
using System.IO;

namespace SSM
{
    /// <summary>
    /// Manages Steam's screenshot folder.
    /// </summary>
    class Manager
    {
        /// <summary>
        /// The path to the screenshots folder.
        /// </summary>
        public string BasePath { get; private set; }

        /// <summary>
        /// Gets a list of file name prefixes that are to be ignored.
        /// </summary>
        public List<ulong> IgnoredPrefixes { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SSM.Manager"/> class with the specified base path.
        /// </summary>
        /// <param name="path">The path the screenshots folder.</param>
        public Manager(string path)
        {
            BasePath = path;
            IgnoredPrefixes = new List<ulong>();
        }

        /// <summary>
        /// Moves all uncategorized screenshots into their respective subfolders.
        /// </summary>
        public void Move()
        {
            foreach (string path in Directory.EnumerateFiles(this.BasePath, "*.png", SearchOption.TopDirectoryOnly))
            {
                string fileName = Path.GetFileName(path);
                string name = GetNameFromFile(fileName);
                if (ShouldSkip(name))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(Properties.Resources.SkippedFile, fileName);
                    Console.ResetColor();
                }
                else
                {
                    string newDir = Path.Combine(this.BasePath, name);
                    string newPath = Path.Combine(newDir, fileName);
                    Console.WriteLine("{0} => {1}", fileName, name);

                    if (!Directory.Exists(newDir))
                        Directory.CreateDirectory(newDir);
                    File.Move(path, newPath);
                }
            }
        }

        /// <summary>
        /// Returns whether a file should be skipped or not.
        /// </summary>
        /// <param name="file">The name of the file (without path).</param>
        /// <returns>True if the file should be skipped.</returns>
        public bool ShouldSkip(string file)
        {
            if (string.IsNullOrEmpty(file)) return true;

            foreach (var item in IgnoredPrefixes)
            {
                if (file.StartsWith(item.ToString()))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines the subfolder name for the specified filename.
        /// </summary>
        /// <param name="file">The name of the file without path.</param>
        /// <returns>A string with a game name or an empty string.</returns>
        private string GetNameFromFile(string file)
        {
            int pos = file.IndexOf('_');
            if (pos > 0)
            {
                ulong appId = ulong.Parse(file.Substring(0, pos));
                string name = Steam.GetAppName(appId);
                if (string.IsNullOrWhiteSpace(name))
                {
                    name = PromptName(file, appId);
                    if (name == null) 
                        return string.Empty;
                }

                foreach (char invalid in Path.GetInvalidFileNameChars())
                    name = name.Replace(invalid.ToString(), "");
                foreach (char invalid in new char[] { '™' })
                    name = name.Replace(invalid.ToString(), "");

                return name;
            }

            return string.Empty;
        }

        /// <summary>
        /// Prompts the user to specify a name for a file.
        /// </summary>
        /// <param name="file">The file name without path of a screenshot to show.</param>
        /// <param name="appId">The App ID for the file.</param>
        /// <returns>A name, or null.</returns>
        private string PromptName(string file, ulong appId)
        {
            string name = null;

            using (UnknownAppIdDialog d = new UnknownAppIdDialog(Path.Combine(this.BasePath, file)))
            {
                if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    name = d.GameName;
                    Steam.SetAppName(appId, name);
                }
                else
                {
                    IgnoredPrefixes.Add(appId);
                }
            }

            return name;
        }
    }
}
