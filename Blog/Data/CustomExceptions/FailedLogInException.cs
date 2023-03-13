using System;
using System.Runtime.Serialization;

namespace Blog.Data.CustomExceptions
{
    [Serializable]
    public class FailedLogInException :BaseException
    {
        public FailedLogInException() : base("UserName or Password is Incorrect, Please Try Again!") { }
        protected FailedLogInException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext) { }
    }
}
