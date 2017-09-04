using System.IO;

namespace Embedder
{
    public class EmbeddedExtensionsWriter
    {
        public const string FileName = "EmbeddedExtensions.cs";

        private const string ClassText = @"using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class ArraySegmentByteExtensions
    {
        public static string Utf8ToString(this ArraySegment<byte> segment)
        {
            return Encoding.UTF8.GetString(segment.Array, segment.Offset, segment.Count);
        }

    }
}

namespace System.IO
{
    public static class StreamExtensions
    {
        public static Task WriteAsync(this Stream stream, ArraySegment<byte> segment)
        {
            return stream.WriteAsync(segment.Array, segment.Offset, segment.Count);
        }
    }
}
";

        public static void WriteEmbeddedExtensionsClass(string directory, string @namespace)
        {
            using (var writer = File.CreateText(Path.Combine(directory, "EmbeddedExtensions.cs")))
            {
                writer.Write(ClassText.Replace("{{namespace}}", @namespace));
            }
        }
    }
}