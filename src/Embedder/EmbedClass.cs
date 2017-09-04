using System.Collections.Generic;
using Newtonsoft.Json;

namespace Embedder
{
    public class EmbedClass
    {
        public EmbedConfig Config { get; set; }
        public EmbedClass(string name)
        {
            Name = name;
        }

        public string Namespace { get; set; }
        public string Name { get; }

        [JsonIgnore]
        public List<EmbedProperty> Properties { get; set; }
    }
}