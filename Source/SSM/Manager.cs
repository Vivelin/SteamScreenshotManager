using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public Manager(string path)
    {
      this.BasePath = path;
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
        if (!string.IsNullOrEmpty(name))
        {
          string newDir = Path.Combine(this.BasePath, name);
          string newPath = Path.Combine(newDir, fileName);
          Console.WriteLine("{0} => {1}", fileName, name);

          if (!Directory.Exists(newDir))
            Directory.CreateDirectory(newDir);
          File.Move(path, newPath);
        }
        else
        {
          Console.ForegroundColor = ConsoleColor.Yellow;
          Console.WriteLine("Skipped {0}!", fileName);
          Console.ResetColor();
        }
      }
    }

    /// <summary>
    /// Determines the subfolder name for the specified filename.
    /// </summary>
    /// <param name="file">The name of the file without path.</param>
    /// <returns>A string with a game name, unparsed ID, or an empty string.</returns>
    private string GetNameFromFile(string file)
    {
      int pos = file.IndexOf('_');
      if (pos > 0)
      {
        string name = file.Substring(0, pos);
        try
        {
          int appId = int.Parse(name);
          name = Steam.GetAppName(appId);
        }
        catch { }

        foreach (char invalid in Path.GetInvalidFileNameChars())
          name = name.Replace(invalid.ToString(), "");
        foreach (char invalid in new char[] { '™' })
          name = name.Replace(invalid.ToString(), "");

        return name;
      }

      return string.Empty;
    }
  }
}
