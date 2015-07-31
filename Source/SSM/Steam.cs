using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace SSM
{
    /// <summary>
    /// Provides static functions relating to Steam.
    /// </summary>
    public class Steam
    {
        private const int WEBCONTENT_MAX_LENGTH = 4096;

        private static string appBaseUrl = "http://steamcommunity.com/app/{0}";
        private static Dictionary<ulong, string> appNames;

        /// <summary>
        /// Initializes static fields and properties.
        /// </summary>
        static Steam()
        {
            Steam.appNames = new Dictionary<ulong, string>();
        }

        /// <summary>
        /// Gets the name of a Steam game with the specified ID.
        /// </summary>
        /// <param name="id">The App ID to find a name for.</param>
        /// <returns>
        /// A string containing the display name of the game with the ID, or an
        /// empty string.
        /// </returns>
        public static string GetAppName(ulong id)
        {
            if (Steam.appNames.ContainsKey(id))
            {
                return Steam.appNames[id];
            }
            else
            {
                string name = GetAppNameInternal(id);
                if (!string.IsNullOrWhiteSpace(name))
                {
                    Steam.appNames.Add(id, name);
                    return name;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Associates an App ID with the specified name.
        /// </summary>
        /// <param name="id">The App ID.</param>
        /// <param name="name">The name that belongs to the App ID.</param>
        public static void SetAppName(ulong id, string name)
        {
            if (Steam.appNames.ContainsKey(id))
                Steam.appNames[id] = name;
            else
                Steam.appNames.Add(id, name);
        }

        /// <summary>
        /// Makes a web request to the Steam Community to retrieve a name, and
        /// prompts the user in case the request fails.
        /// </summary>
        /// <param name="id">The App ID to find a name for.</param>
        /// <returns>
        /// A string containing the display name of the game with the ID, or an
        /// empty string.
        /// </returns>
        private static string GetAppNameInternal(ulong id)
        {
            string name = null;
            WebClient client = new WebClient();

            int ticks = Environment.TickCount;
            string content = client.DownloadString(string.Format(appBaseUrl, id));
            Console.WriteLine(Properties.Resources.RequestCompletedIn, id, (Environment.TickCount - ticks));

            if (content != null)
            {
                if (content.Length > WEBCONTENT_MAX_LENGTH) content = content.Substring(0, WEBCONTENT_MAX_LENGTH);
                if (!TryParseAppPage(content, out name))
                {
                    name = null;
                }
            }

            return name;
        }

        /// <summary>
        /// Parses the specified content for the page title.
        /// </summary>
        /// <param name="content">
        /// A string containing the HTML content of the page to parse.
        /// </param>
        /// <param name="name">The parsed name, or null.</param>
        /// <returns>True if content was parsed successfully.</returns>
        private static bool TryParseAppPage(string content, out string name)
        {
            try
            {
                Match m = Regex.Match(content, "<title>(.*) :: ?(.*)</title>", RegexOptions.IgnoreCase);
                if (m.Groups.Count > 2)
                {
                    name = m.Groups[2].Value.Trim();

                    byte[] data = Encoding.Default.GetBytes(name);
                    name = Encoding.UTF8.GetString(data);
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }

            name = null;
            return false;
        }
    }
}
