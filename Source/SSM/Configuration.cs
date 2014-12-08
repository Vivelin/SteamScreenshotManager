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
    class Configuration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> 
        /// class.
        /// </summary>
        public Configuration()
        {
            BaseDir = null;
        }

        /// <summary>
        /// Gets or sets the full path to the file this configuration 
        /// represents.
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the location of the screenshot folder.
        /// </summary>
        [DisplayName("Screenshot folder")]
        [Description("The location of Steam's external screenshot folder.")]
        public string BaseDir { get; set; }

        /// <summary>
        /// Saves the configuration to the file it was loaded from.
        /// </summary>
        public void Save()
        {
            string dir = Path.GetDirectoryName(this.FileName);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            var json = JsonConvert.SerializeObject(this);
            File.WriteAllText(FileName, json);
        }

        /// <summary>
        /// Creates a <see cref="Configuration"/> from the specified file.
        /// </summary>
        /// <param name="path">The path to the file to load.</param>
        /// <returns>A new <see cref="Configuration"/> class.</returns>
        public static Configuration FromFile(string path)
        {
            var config = new Configuration();

            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                config = JsonConvert.DeserializeObject<Configuration>(json);
            }

            config.FileName = path;
            return config;
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
