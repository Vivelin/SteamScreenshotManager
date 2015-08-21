using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace SSM
{
    /// <summary>
    /// Represents the main entry point for the application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The path to the AppData directory.
        /// </summary>
        public static readonly string AppData =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        /// <summary>
        /// The path to the directory that contains the configuration files.
        /// </summary>
        public static readonly string ConfigDir =
            Path.Combine(AppData, "SteamScreenshotManager");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static int Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var configFile = Path.Combine(ConfigDir, "Config.json");
            var cacheFile = Path.Combine(ConfigDir, "NameCache.json");

            var config = Configuration.FromFile(configFile);
            CheckSettings(config);

            var manager = new Manager(config.BaseDir);
            manager.FolderNameCache = NameCache.FromFile(cacheFile);

            manager.Move();
            manager.FolderNameCache.Save();

            Console.WriteLine("Cave Johnson, we're done here.");
            return 0;
        }

        /// <summary>
        /// Checks whether the specified configuration is valid and can be used.
        /// If the configuration is invalid, an appropiate exception is thrown.
        /// Otherwise, the function simply returns.
        /// </summary>
        /// <param name="config">
        /// The <see cref="Configuration"/> object to check.
        /// </param>
        private static void CheckSettings(Configuration config)
        {
            if (!Directory.Exists(config.BaseDir))
            {
                string foundDir = FindBaseDir();
                if (foundDir != null && Directory.Exists(foundDir))
                    config.BaseDir = foundDir;
                else
                    throw new Exception("No screenshot folder has been set.");
            }

            config.Save();
        }

        /// <summary>
        /// Finds and returns Steam's external screenshots folder.
        /// </summary>
        /// <returns>The full path to the folder, or null.</returns>
        private static string FindBaseDir()
        {
            // TODO: Parse Steam userdata config to find uncompressed
            //       screenshots folder
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
                dialog.Title = "Please select your external screenshots folder";
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    return dialog.FileName;
                }
            }

            return null;
        }
    }
}
