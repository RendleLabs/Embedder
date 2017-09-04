using System.IO;
using System.Text;

namespace Embedder.Stringifiers
{
    public class TextFileLoader : IFileLoader
    {
        public int LoadFile(string sourceFile, Stream stream)
        {
            using (var reader = File.OpenText(sourceFile))
            {
                var text = reader.ReadToEnd();
                var bytes = Encoding.UTF8.GetBytes(text);
                stream.Write(bytes, 0, bytes.Length);
                return bytes.Length;
            }
        }
    }
}