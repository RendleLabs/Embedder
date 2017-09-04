using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Embedder.Tests
{
    public class EmbedConfigParserTests
    {
        static List<EmbedClass> Parse(string json)
        {
            var source = JObject.Parse(json);
            var target = new EmbedConfigParser();
            return target.ParseClasses(source, null).ToList();
        }

        [Fact]
        public void ReturnsClassesForTopLevelProperties()
        {
            var actual = Parse(@"{""Foo"": {}, ""Bar"": {}}");
            Assert.Equal(2, actual.Count);
            Assert.Contains(actual, c => c.Name == "Foo");
            Assert.Contains(actual, c => c.Name == "Bar");
        }

        [Fact]
        public void ParsesProperties()
        {
            var embedClass =
                Parse(
                        @"{ ""Foo"": { ""Bar"": { ""File"": ""Web/Index.html"", ""FileType"": ""Text"", ""PropertyType"": ""byte[]"" } } }")
                    .Single();
            var actual = embedClass.Properties.SingleOrDefault();
            Assert.NotNull(actual);
            Assert.Equal("Web/Index.html", actual.File);
            Assert.Equal("Text", actual.FileType);
        }
    }
}
