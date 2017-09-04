using System;
using System.Runtime.Serialization;

namespace Embedder
{
    [Serializable]
    public class FatalException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public FatalException()
        {
        }

        public FatalException(string message) : base(message)
        {
        }

        public FatalException(string message, Exception inner) : base(message, inner)
        {
        }

        protected FatalException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}