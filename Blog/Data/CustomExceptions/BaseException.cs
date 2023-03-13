using System.Runtime.Serialization;

namespace Blog.Data.CustomExceptions
{
    [Serializable]
    public class BaseException :Exception
    {
        public BaseException() 
            :base() { }

        public BaseException(string message)
            : base(message) { }
        protected BaseException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext) { }
    }
}
