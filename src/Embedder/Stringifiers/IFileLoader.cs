using System.IO;

namespace Embedder.Stringifiers
{
    public interface IFileLoader
    {
        int LoadFile(string sourceFile, Stream stream);
    }
}