using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Embedder
{
    public class EmbedConfigParser
    {
        public EmbedConfig Parse(JObject source)
        {
            var config = source.ToObject<EmbedConfig>();

            if (string.IsNullOrWhiteSpace(config.Namespace))
            {
                config.Namespace = InferNamespace(config.Directory);
            }
            if (source["TextFileExtensions"] is JArray textExtensions)
            {
                config.TextFileExtensions = new HashSet<string>(textExtensions.ToObject<string[]>());
            }
            if (source["Classes"] is JObject classesSource)
            {
                config.Classes = ParseClasses(classesSource, config).ToList();
            }
            return config;
        }

        private static string InferNamespace(string embeddedDirectory)
        {
            var extra = Regex.Replace(embeddedDirectory, @"[/\\]", ".");
            var csproj = Directory.EnumerateFiles(Environment.CurrentDirectory, "*.csproj").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(csproj))
            {
                return $"{Path.GetFileNameWithoutExtension(csproj)}.{extra}";
            }
            return $"{Path.GetDirectoryName(Environment.CurrentDirectory)}.{extra}";
        }

        public IEnumerable<EmbedClass> ParseClasses(JObject source, EmbedConfig config)
        {
            foreach (var property in source.Properties())
            {
                var embedClass = new EmbedClass(property.Name)
                {
                    Config = config
                };

                if (property.Value is JObject props)
                {
                    embedClass.Properties = ParseProperties(props, embedClass).ToList();
                }
                yield return embedClass;
            }
        }

        private static IEnumerable<EmbedProperty> ParseProperties(JObject source, EmbedClass embedClass)
        {
            foreach (var property in source.Properties())
            {
                if (property.Value is JObject config)
                {
                    var embedProperty = config.ToObject<EmbedProperty>();
                    embedProperty.Class = embedClass;
                    embedProperty.Name = property.Name;
                    yield return embedProperty;
                }
                if (property.Value.Type == JTokenType.String)
                {
                    yield return new EmbedProperty
                    {
                        Class = embedClass,
                        Name = property.Name,
                        File = property.Value.ToString()
                    };
                }
            }
        }
    }
}