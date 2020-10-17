using System;

using System.IO;
using System.IO.Compression;

using System.Net;

using System.Threading.Tasks;

namespace ShellRobot.Shared
{
    /// <summary>
    /// Entry point for <see cref="ShellRobot"/>.
    /// </summary>
    public class ShellRobot
    {
        /// <summary>
        /// ShellRobot repository for download templates.
        /// </summary>
        private const string Repository = "https://github.com/BizarreNULL/shell-robot-templates/archive/main.zip";

        /// <summary>
        /// Release file of given repository.
        /// </summary>
        private const string Release =
            "https://raw.githubusercontent.com/BizarreNULL/shell-robot-templates/main/release";

        private readonly string _home = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SHELLROBOT_HOME"))
            ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".shellRobot")
            : Environment.GetEnvironmentVariable("SHELLROBOT_HOME");

        /// <summary>
        /// Default constructor for <see cref="ShellRobot"/>.
        /// </summary>
        public ShellRobot()
        {
            if (!Directory.Exists(_home))
            {
                Directory.CreateDirectory(_home);
            }
        }

        /// <summary>
        /// Check if is a newer version of templates repository.
        /// </summary>
        /// <returns>True if exist.</returns>
        public async Task<bool> CheckUpdateAsync()
        {
            if (File.Exists(Path.Combine(_home, "release")) is false)
            {
                return true;
            }

            var version = await File.ReadAllTextAsync(Path.Combine(_home, "release"));
            var remoteVersion = await new WebClient().DownloadStringTaskAsync(Release);

            return int.Parse(version) < int.Parse(remoteVersion);
        }

        /// <summary>
        /// Update current application templates.
        /// </summary>
        /// <returns></returns>
        public async Task UpdateAsync()
        {
            Directory.Delete(_home, true);

            var source = Path.Combine(Path.GetTempPath(), $"{new Guid().ToString()}.zip");
            await new WebClient().DownloadFileTaskAsync(Repository, source);

            ZipFile.ExtractToDirectory(source, Path.GetTempPath());
            File.Delete(source);

            Directory.Move(Path.Combine(Path.GetTempPath(), "shell-robot-templates-main"), _home);
        }
    }
}