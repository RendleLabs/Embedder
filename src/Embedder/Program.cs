using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Embedder
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0].Equals("-i") || args[0].Equals("--init"))
                {
                    new Initializer(args.Skip(1)).WriteConfigFile();
                    Environment.Exit(0);
                }
            }
            try
            {
                var configSource = await LoadConfig();
                var config = new EmbedConfigParser().Parse(configSource);

                var path = Path.Combine(Environment.CurrentDirectory, config.Directory);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (!File.Exists(Path.Combine(path, EmbeddedExtensionsWriter.FileName)))
                {
                    EmbeddedExtensionsWriter.WriteEmbeddedExtensionsClass(path, config.Namespace);
                }
                foreach (var embedClass in config.Classes)
                {
                    embedClass.Namespace = config.Namespace;
                    var classPath = Path.Combine(path, $"{embedClass.Name}.cs");
                    using (var generator = new Generator(classPath))
                    {
                        generator.Generate(embedClass);
                    }
                }
            }
            catch (NonFatalException e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }
            catch (FatalException e)
            {
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }

        static async Task<JObject> LoadConfig()
        {
            var configFilePath = Path.Combine(Environment.CurrentDirectory, "embedconfig.json");
            if (!File.Exists(configFilePath))
            {
                throw new NonFatalException("No embedconfig.json file found.");
            }

            string configJson;
            try
            {
                using (var stream = File.OpenRead(configFilePath))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        configJson = await reader.ReadToEndAsync();
                    }
                }

            }
            catch (Exception)
            {
                throw new FatalException("Failed to read embedconfig.json.");
            }

            if (string.IsNullOrWhiteSpace(configJson))
            {
                throw new NonFatalException("Empty embedconfig.json file.");
            }

            try
            {
                return JObject.Parse(configJson);
            }
            catch (Exception)
            {
                throw new FatalException("Failed to parse embedconfig.json.");
            }
        }
    }
}
