using System;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;

namespace SSM
{
    /// <summary>
    /// Represents a configuration for the application.
    /// </summary>
    [Serializable]
    public class Configuration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Configuration()
        {
            BaseDir = null;
        }

        /// <summary>
        /// Gets or sets the location of the screenshot folder.
        /// </summary>
        [DisplayName("Screenshot folder"), Category("General")]
        [Description("The location of Steam's external screenshot folder.")]
        public string BaseDir { get; set; }

        /// <summary>
        /// Gets or sets the full path to the file this configuration
        /// represents.
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets a regular expression that determines which files to
        /// process and upload.
        /// </summary>
        [DisplayName("Filter"), Category("General")]
        [Description("A regular expression that determines which files to process and upload.")]
        [DefaultValue("(\\.png|\\.jpe?g)$")]
        public string Filter { get; set; } = "(\\.png|\\.jpe?g)$";

        /// <summary>
        /// Gets or sets the path to the private key file used to authenticate
        /// with the remote host.
        /// </summary>
        [DisplayName("Private key file"), Category("Sync")]
        [Description("The location of the private key file used to authenticate with the remote host.")]
        public string PrivateKeyFilePath { get; set; }

        /// <summary>
        /// Gets or sets the path to the screenshots folder on the remote host.
        /// </summary>
        [DisplayName("Remote screenshots folder"), Category("Sync")]
        [Description("The location of the screenshots folder on the remote host.")]
        public string RemoteBaseDir { get; set; }

        /// <summary>
        /// Gets or sets the name or IP address of the remote host.
        /// </summary>
        [DisplayName("Host name"), Category("Sync")]
        [Description("The name or IP address of the remote host.")]
        public string RemoteHost { get; set; }

        /// <summary>
        /// Gets or sets the number of the public port used to connect to the
        /// server on the remote host.
        /// </summary>
        [DisplayName("Port"), Category("Sync")]
        [Description("The port number of the server on the remote host.")]
        [DefaultValue(22)]
        public int RemotePort { get; set; } = 22;

        /// <summary>
        /// Gets or sets the user name used to log in to the remote host.
        /// </summary>
        [DisplayName("User name"), Category("Sync")]
        [Description("The user name used to log in to the remote host.")]
        public string RemoteUser { get; set; }

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
        /// Saves the configuration to the file it was loaded from.
        /// </summary>
        public void Save()
        {
            string dir = Path.GetDirectoryName(this.FileName);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                DefaultValueHandling = DefaultValueHandling.Include
                /* Use the value below when documentation is available and when
                 * there are a lot of settings that will likely have their
                 * default values.
                 */
                // DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
            };
            var json = JsonConvert.SerializeObject(this, settings);
            File.WriteAllText(FileName, json);
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
