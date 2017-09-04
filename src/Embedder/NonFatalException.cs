using System;
using System.Runtime.Serialization;

namespace Embedder
{
    [Serializable]
    public class NonFatalException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public NonFatalException()
        {
        }

        public NonFatalException(string message) : base(message)
        {
        }

        public NonFatalException(string message, Exception inner) : base(message, inner)
        {
        }

        protected NonFatalException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}