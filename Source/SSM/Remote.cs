using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Renci.SshNet;

namespace SSM
{
    public class Remote
    {
        public Remote(Configuration config)
            : this(config, config.RemoteHost, config.RemoteUser,
                  config.RemotePort, config.PrivateKeyFilePath)
        { }

        public Remote(Configuration config, string host, string userName, int port, string keyfilePath)
        {
            var key = new PrivateKeyFile(keyfilePath);

            Config = config;
            ConnectionInfo = new PrivateKeyConnectionInfo(host, port, userName, key);
        }

        public Configuration Config { get; set; }
        public ConnectionInfo ConnectionInfo { get; set; }

        public void Upload(string localPath, string remotePath)
        {
            if (string.IsNullOrEmpty(localPath))
                throw new ArgumentNullException(nameof(localPath));
            if (string.IsNullOrEmpty(remotePath))
                throw new ArgumentNullException(nameof(remotePath));

            if (localPath.EndsWith("\\") || localPath.EndsWith("/"))
                localPath = Path.GetDirectoryName(localPath);
            if (remotePath.EndsWith("\\") || remotePath.EndsWith("/"))
                remotePath = Path.GetDirectoryName(remotePath);

            using (var client = new SftpClient(ConnectionInfo))
            {
                client.Connect();
                client.ChangeDirectory(remotePath);

                var folders = Directory.EnumerateDirectories(localPath);
                foreach (var path in folders)
                {
                    var name = Path.GetFileName(path);
                    var remote = Path.Combine(remotePath, name).Replace('\\', '/');
                    Upload(path, remote, client);
                }
            }
        }

        protected void Upload(string localPath, string remotePath, SftpClient client)
        {
            var folderName = Path.GetFileName(localPath);
            Trace.WriteLine($"Syncing \"{folderName}\"", "Information");

            if (!client.Exists(remotePath))
                client.CreateDirectory(remotePath);
            client.ChangeDirectory(remotePath);

            var files = Directory.EnumerateFiles(localPath);
            foreach (var path in files)
            {
                if (!Regex.IsMatch(path, Config.Filter, RegexOptions.IgnoreCase))
                    continue;

                var fileName = Path.GetFileName(path);
                if (client.Exists(fileName))
                {
                    // TODO: Compare last write time
                    Trace.WriteLine($"File exists: \"{fileName}\"", "Verbose");
                    continue;
                }

                using (var file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    Trace.WriteLine($"Syncing \"{fileName}\"...", "Information");
                    client.UploadFile(file, fileName);
                }
            }

            var folders = Directory.EnumerateDirectories(localPath);
            foreach (var path in folders)
            {
                var name = Path.GetFileName(path);
                var remote = Path.Combine(remotePath, name).Replace('\\', '/');
                Upload(path, remote, client);
            }

            client.ChangeDirectory("..");
        }
    }
}
