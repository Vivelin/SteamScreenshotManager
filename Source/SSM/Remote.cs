using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Renci.SshNet;

namespace SSM
{
    /// <summary>
    /// Represents a remote server.
    /// </summary>
    public class Remote : IDisposable
    {
        private bool disposedValue = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Remote"/> class using
        /// the specified configuration.
        /// </summary>
        /// <param name="config">
        /// A <see cref="Configuration"/> object specifying the server
        /// connection settings.
        /// </param>
        public Remote(Configuration config)
            : this(config, config.RemoteHost, config.RemoteUser,
                  config.RemotePort, config.PrivateKeyFilePath)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Remote"/> class using
        /// the specified configuration and connection settings.
        /// </summary>
        /// <param name="config">A <see cref="Configuration"/> object.</param>
        /// <param name="host">
        /// The name or IP address of the remote host.
        /// </param>
        /// <param name="userName">
        /// The user name used to log in to the remote host.
        /// </param>
        /// <param name="port">
        /// The public port used to connect to the server on the remote host.
        /// </param>
        /// <param name="keyfilePath">
        /// The path to the private key file used to authenticate with the
        /// remote host.
        /// </param>
        public Remote(Configuration config, string host, string userName,
            int port, string keyfilePath)
        {
            var key = new PrivateKeyFile(keyfilePath);

            Config = config;
            ConnectionInfo = new PrivateKeyConnectionInfo(host, port, userName,
                key);
        }

        /// <summary>
        /// Gets or sets a <see cref="Configuration"/> object specifying
        /// additional user-configurable settings.
        /// </summary>
        public Configuration Config { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="ConnectionInfo"/> object specifying the
        /// connection settings used to connect to the remote host.
        /// </summary>
        public ConnectionInfo ConnectionInfo { get; protected set; }

        /// <summary>
        /// Cleans up resources used by this instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Uploads all files from the local path to the specified remote path.
        /// </summary>
        /// <param name="localPath">
        /// The base path on the local machine containing the files to upload.
        /// </param>
        /// <param name="remotePath">
        /// The base destination path on the remote server.
        /// </param>
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

        /// <summary>
        /// Cleans up unmanaged resources and optionally cleans up managed
        /// resources.
        /// </summary>
        /// <param name="disposing">
        /// Indicates whether to dispose of managed resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (ConnectionInfo != null &&
                        ConnectionInfo is PrivateKeyConnectionInfo)
                    {
                        // Fuck you, SSH.Net.
                        ((PrivateKeyConnectionInfo)ConnectionInfo).Dispose();
                        ConnectionInfo = null;
                    }
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Uploads all files from the local path to the specified remote path.
        /// </summary>
        /// <param name="localPath">
        /// The current path on the local machine containing the files to
        /// upload.
        /// </param>
        /// <param name="remotePath">
        /// The destination path on the remote server. If the top-level
        /// directory does not exist, it will be created.
        /// </param>
        /// <param name="client">
        /// An <see cref="SftpClient"/> object that represents the connection to
        /// the remote server.
        /// </param>
        protected void Upload(string localPath, string remotePath,
            SftpClient client)
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

                using (var file = new FileStream(path, FileMode.Open,
                    FileAccess.Read, FileShare.Read))
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
