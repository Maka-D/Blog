using System.Runtime.Serialization;

namespace Blog.Data.CustomExceptions
{
    [Serializable]
    public class FailedRegistrationException :BaseException
    {
        public FailedRegistrationException() : base("Registration Failed, Please Try Again!") { }
        public FailedRegistrationException(string message) : base(message) { }
        protected FailedRegistrationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext) { }

    }
}
