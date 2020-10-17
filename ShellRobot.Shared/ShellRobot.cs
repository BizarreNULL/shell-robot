using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ShellRobot.Shared.Models;

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

        /// <summary>
        /// List available reverse shell templates.
        /// </summary>
        /// <returns>Array containing the list of available templates.</returns>
        public async Task<List<Template>> ListTemplatesAsync()
        {
            var templates = new List<Template>();

            foreach (var path in Directory.GetFiles(_home, "*.json"))
            {
                var template = await JsonSerializer.DeserializeAsync<Template>(File.OpenRead(path));
                templates.Add(template);
            }

            return templates;
        }

        /// <summary>
        /// Parse a template by given title with their specific params.
        /// </summary>
        /// <param name="title">Title of an existent template on current system.</param>
        /// <param name="args">Arguments for given template.</param>
        /// <returns>Parsed reverse shell string</returns>
        /// <exception cref="ArgumentException">If title not exist in any available template.</exception>
        /// <exception cref="ArgumentException">If args don't match with rules of given template.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If args is out of range of given template.</exception>
        public async Task<string> ParseTemplateAsync(string title, string[] args)
        {
            var templates = await ListTemplatesAsync();
            var template = templates.FirstOrDefault(t =>
                string.Equals(t.Title, title, StringComparison.InvariantCultureIgnoreCase));

            if (template == null)
            {
                throw new ArgumentException("No template matches for given title.", nameof(title));
            }

            if (template.Args
                .Where(arg => arg.Value.Mandatory)
                .Any(arg => !args.Contains(arg.Key)))
            {
                throw new ArgumentException("One or more mandatory argument is missing.", nameof(args));
            }
            
            foreach (var (name, param) in template.Args)
            {
                var index = Array.IndexOf(args, name);

                if (index + 1 > args.Length || index == -1)
                {
                    throw new ArgumentOutOfRangeException(nameof(args),
                        $"Missing value for mandatory argument {name}.");
                }

                var value = args[index + 1];

                if (param.Mandatory && string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException($"Value for mandatory argument \"{name}\" is missing,",
                        nameof(args));
                }

                if (param.Validation.Expressions.Any() && !param.Validation.AnyMatch)
                {
                    foreach (var expression in param.Validation.Expressions)
                    {
                        foreach (var regex in expression.Regexes)
                        {
                            if (!Regex.IsMatch(value, regex))
                            {
                                throw new ArgumentException(
                                    $"Value \"{value}\" don't matches with expression {regex}.",
                                    nameof(value));
                            }
                        }
                    }
                }
                else
                {
                    foreach (var expression in param.Validation.Expressions)
                    {
                        var matches = expression.Regexes.Count(regex => 
                            Regex.IsMatch(value, regex));

                        if (matches == 0)
                        {
                            throw new ArgumentException(
                                $"Value \"{value}\" don't matches with any expressions of {expression.Description}.",
                                nameof(value));
                        }
                    }
                }

                template.Content = template.Content
                    .Replace(param.Pattern, value);
            }

            return template.Content;
        }
    }
}