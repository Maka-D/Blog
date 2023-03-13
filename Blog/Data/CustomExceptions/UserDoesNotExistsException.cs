using System.Runtime.Serialization;

namespace Blog.Data.CustomExceptions
{
    [Serializable]
    public class UserDoesNotExistsException :BaseException
    {
        public UserDoesNotExistsException() : base("User Does Not Exists!") { }
        protected UserDoesNotExistsException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext) { }
    }
}
