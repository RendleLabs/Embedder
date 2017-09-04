using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Embedder
{
    public class Initializer
    {
        private static readonly string[] TextExtensions = { "txt", "html", "htm", "js", "cs", "svg", "json", "xml" };
        private readonly string[] _directories;
        private StreamWriter _writer;

        public Initializer(IEnumerable<string> directories)
        {
            _directories = directories.ToArray();
        }

        public void WriteConfigFile()
        {
            var configFile = Path.Combine(Environment.CurrentDirectory, "embedconfig.json");
            if (File.Exists(configFile))
            {
                Console.Error.WriteLine("Configuration file already exists.");
                Environment.Exit(2);
            }

            if (_directories.Length == 0)
            {
                Console.WriteLine("Usage: dotnet embed --init directory [directory [...]]");
                Environment.Exit(3);
            }

            using (_writer = File.CreateText(configFile))
            {
                _writer.WriteLine("{");
                _writer.WriteLine("  \"Directory\": \"Embedded\",");
                _writer.WriteLine($"  \"TextFileExtensions\": [\"{string.Join("\",\"", TextExtensions)}\"],");
                _writer.WriteLine("  \"Classes\": {");
                bool first = true;
                foreach (var directory in _directories)
                {
                    if (!first)
                    {
                        _writer.WriteLine("    },");
                    }
                    else
                    {
                        first = false;
                    }
                    if (Directory.Exists(Path.Combine(Environment.CurrentDirectory, directory)))
                    {
                        var className = SanitizeName(directory, Environment.CurrentDirectory);
                        _writer.WriteLine($"    \"{className}\": {{");

                        var props = string.Join($",{Environment.NewLine}", FileProperties(directory, Path.Combine(Environment.CurrentDirectory, directory)));
                        _writer.WriteLine(props);
                    }
                }
                _writer.WriteLine("    }");
                _writer.WriteLine("  }");
                _writer.WriteLine("}");
            }
        }

        private static IEnumerable<string> FileProperties(string directory, string root)
        {
            var absolute = Path.Combine(Environment.CurrentDirectory, directory);

            IEnumerable<string> Enumerate()
            {
                foreach (var file in Directory.EnumerateFiles(absolute))
                {
                    var name = SanitizeName(file, root);
                    var path = Path.GetRelativePath(Environment.CurrentDirectory, file).Replace('\\', '/');
                    yield return $"      \"{name}\": \"{path}\"";
                }
            }

            return Enumerate()
                .Concat(Directory.EnumerateDirectories(directory).Select(d => FileProperties(d, root)).SelectMany(e => e));
        }

        private static string SanitizeName(string name, string root)
        {
            name = Path.GetRelativePath(root, name);
            name = Regex.Replace(name, "[^0-9a-z]", "_", RegexOptions.IgnoreCase);
            if (Regex.IsMatch(name, "^[0-9]"))
            {
                name = $"_{name}";
            }
            return name;
        }
    }
}