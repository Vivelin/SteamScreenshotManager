using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;

namespace SSM
{
    /// <summary>
    /// Represents a cached string dictionary that maps app IDs or names to
    /// folder names.
    /// </summary>
    [JsonDictionary]
    class NameCache : Dictionary<string, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NameCache"/> class.
        /// </summary>
        public NameCache()
        {

        }

        /// <summary>
        /// Gets or sets the name of the file that contains the map's data.
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>
        /// The value associated with the specified key. If the specified key 
        /// is not found, a get operation returns <c>null</c>, and a set 
        /// operation creates a new element with the specified key.
        /// </returns>
        public new string this[string key]
        {
            get
            {
                if (ContainsKey(key))
                    return base[key];
                return null;
            }
            set { base[key] = value; }
        }

        /// <summary>
        /// Creates a <see cref="NameCache"/> from the specified file.
        /// </summary>
        /// <param name="path">The path to the file to load.</param>
        /// <returns>
        /// A new <see cref="NameCache"/> object with the <see cref="FileName"/>
        /// property set to <paramref name="path"/>.
        /// </returns>
        public static NameCache FromFile(string path)
        {
            var nameCache = new NameCache();

            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                nameCache = JsonConvert.DeserializeObject<NameCache>(json);
            }

            nameCache.FileName = path;
            return nameCache;
        }

        /// <summary>
        /// Saves the cache's content to disk.
        /// </summary>
        public void Save()
        {
            var dir = Path.GetDirectoryName(FileName);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(FileName, json);
        }
    }
}
