using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SSM
{
  /// <summary>
  /// Provides static functions relating to Steam.
  /// </summary>
  class Steam
  {
    private const int WEBCONTENT_MAX_LENGTH = 4096;

    private static Dictionary<int, string> appNames;
    private static string appBaseUrl = "http://steamcommunity.com/app/{0}";

    /// <summary>
    /// Initializes static fields and properties.
    /// </summary>
    static Steam()
    {
      Steam.appNames = new Dictionary<int, string>();
    }

    /// <summary>
    /// Gets the name of a Steam game with the specified ID.
    /// </summary>
    /// <param name="id">The Game ID of a Steam game.</param>
    /// <returns>A string containing the display name of the game with the ID, or an empty string.</returns>
    public static string GetAppName(int id)
    {
      if (Steam.appNames.ContainsKey(id))
      {
        return Steam.appNames[id];
      }
      else
      {
        WebClient client = new WebClient();

        int ticks = Environment.TickCount;
        string content = client.DownloadString(string.Format(appBaseUrl, id));
        Console.WriteLine("Request for {0} completed in {1} ms", id, (Environment.TickCount - ticks));

        if (content != null)
        {
          if (content.Length > WEBCONTENT_MAX_LENGTH) content = content.Substring(0, WEBCONTENT_MAX_LENGTH);

          Match m = Regex.Match(content, "<title>(.*) :: ?(.*)</title>", RegexOptions.IgnoreCase);
          if (m.Groups.Count > 2)
          {
            string name = m.Groups[2].Value.Trim();

            byte[] data = Encoding.Default.GetBytes(name);
            name = Encoding.UTF8.GetString(data);

            Steam.appNames.Add(id, name);
            return name;
          }
        }
      }
      return string.Empty;
    }
  }
}
