using System.IO;

namespace Embedder.Stringifiers
{
    public class BinaryFileLoader : IFileLoader
    {
        public int LoadFile(string sourceFile, Stream target)
        {
            var source = File.OpenRead(sourceFile);
            long length = source.Length;
            source.CopyTo(target);
            return (int)length;
        }
    }
}