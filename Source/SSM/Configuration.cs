using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace SSM
{
    /// <summary>
    /// Represents a configuration for the application.
    /// </summary>
    [Serializable]
    public class Configuration
    {
        private static string defaultFileName = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class
        /// with the default file name.
        /// </summary>
        public Configuration() : this(DefaultFileName) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class
        /// with the specified file name.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        public Configuration(string fileName)
        {
            FileName = Path.GetFullPath(fileName);
            SetDefaults();
        }

        /// <summary>
        /// Sets properties to their default values.
        /// </summary>
        public void SetDefaults()
        {
            BaseDir = null;
        }

        /// <summary>
        /// Gets the name of default file that is stored in the current user's 
        /// Application Data folder.
        /// </summary>
        public static string DefaultFileName
        {
            get
            {
                if (defaultFileName == null)
                {
                    var appDataFolder = Environment.GetFolderPath(
                        Environment.SpecialFolder.ApplicationData);
                    defaultFileName = Path.Combine(appDataFolder, 
                        "SteamScreenshotManager", "Config.json");
                }
                return defaultFileName;
            }
        }

        /// <summary>
        /// Gets the full path to the file this configuration was loaded from.
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public string FileName { get; private set; }

        /// <summary>
        /// Gets or sets the location of the screenshot folder.
        /// </summary>
        [DisplayName("Screenshot folder")]
        [Description("The location of Steam's external screenshot folder.")]
        public string BaseDir { get; set; }

        /// <summary>
        /// Saves the configuration to the file it was loaded from.
        /// </summary>
        /// <exception cref="System.Exception">Settings could not be saved. Check the inner exception for details.</exception>
        public void Save()
        {
            try
            {
                string dir = Path.GetDirectoryName(this.FileName);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                var json = JsonConvert.SerializeObject(this);
                File.WriteAllText(FileName, json);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Properties.Resources.SaveSettingsFailed, FileName), ex);
            }
        }

        /// <summary>
        /// Loads the <see cref="SSM.Configuration"/> from the default file.
        /// </summary>
        /// <returns>A new instance of the <see cref="SSM.Configuration"/> class, or null.</returns>
        /// <exception cref="System.IO.FileNotFoundException">The default file does not exist.</exception>
        /// <exception cref="System.Exception">Settings could not be loaded. Check the inner exception for details.</exception>
        public static Configuration Load()
        {
            return Load(DefaultFileName);
        }

        /// <summary>
        /// Loads the <see cref="SSM.Configuration"/> from the specified file.
        /// </summary>
        /// <param name="path">The name of the file to load.</param>
        /// <returns>A new instance of the <see cref="SSM.Configuration"/> class, or null.</returns>
        /// <exception cref="System.IO.FileNotFoundException">The file specified by <paramref name="path"/> does not exist.</exception>
        /// <exception cref="System.Exception">Settings could not be loaded. Check the inner exception for details.</exception>
        public static Configuration Load(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException(null, path);

            try
            {
                var json = File.ReadAllText(path);
                var config = JsonConvert.DeserializeObject<Configuration>(json);
                return config;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Properties.Resources.LoadSettingsFailed, path), ex);
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return FileName ?? base.ToString();
        }
    }
}
