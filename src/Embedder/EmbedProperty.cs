using System;
using System.Collections.Generic;
using System.IO;

namespace Embedder
{
    public class EmbedProperty
    {
        private string _fileType;
        public string Name { get; set; }
        public string File { get; set; }

        /// <summary>
        /// Type of the source file. Can be <c>Binary</c> or <c>Text</c>.
        /// </summary>
        public string FileType
        {
            get => string.IsNullOrWhiteSpace(_fileType) ? InferredFileType : _fileType;
            set => _fileType = value;
        }

        public EmbedClass Class { get; set; }

        public bool IsTextFile => "Text".Equals(FileType, StringComparison.OrdinalIgnoreCase);

        public bool IsBinaryFile => "Binary".Equals(FileType, StringComparison.OrdinalIgnoreCase);

        public string InferredFileType =>
            (Class?.Config?.TextFileExtensions ?? TextFileExtensions).Contains(Path.GetExtension(File).TrimStart('.')) ? "Text" : "Binary";

        private static readonly HashSet<string> TextFileExtensions =
            new HashSet<string>(new[] {"txt", "js", "css", "htm", "html", "xhtml", "json", "xml"}, StringComparer.OrdinalIgnoreCase);
    }
}