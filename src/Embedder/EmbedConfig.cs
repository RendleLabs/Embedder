using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Embedder
{
    public class EmbedConfig
    {
        private static readonly HashSet<string> DefaultTextFileExtensions = 
            new HashSet<string>(new[] {"txt", "js", "css", "htm", "html", "xhtml", "json", "xml"}, StringComparer.OrdinalIgnoreCase);

        private HashSet<string> _textFileExtensions = DefaultTextFileExtensions;

        [JsonIgnore]
        public HashSet<string> TextFileExtensions
        {
            get => _textFileExtensions;
            set => _textFileExtensions = value ?? DefaultTextFileExtensions;
        }

        //new HashSet<string>(new[] {"txt", "js", "css", "htm", "html", "xhtml", "json", "xml"}, StringComparer.OrdinalIgnoreCase);
        public string Directory { get; set; } = "Embedded";
        public string Namespace { get; set; }
        [JsonIgnore]
        public List<EmbedClass> Classes { get; set; }
    }
}