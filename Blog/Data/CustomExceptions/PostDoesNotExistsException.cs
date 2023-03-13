using System.Runtime.Serialization;

namespace Blog.Data.CustomExceptions
{
    [Serializable]
    public class PostDoesNotExistsException :BaseException
    {
        public PostDoesNotExistsException() :base("Post Does Not Exists!") { }
        protected PostDoesNotExistsException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext) { }
    }
}
