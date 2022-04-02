using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Business.Errors
{
    [Serializable]
    public class RestException : Exception, ISerializable
    {
        [NonSerialized]
        private readonly HttpStatusCode _errorCode;
        [NonSerialized]
        private readonly string _errorType;
        [NonSerialized]
        private readonly string _errorMessage;

        public RestException(HttpStatusCode statusCode, string message) : base(message)
        {
            _errorCode = statusCode;
            _errorType = statusCode.ToString();
            _errorMessage = message;
        }

        protected RestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _errorCode = (HttpStatusCode)info.GetValue("ErrorCode", typeof(HttpStatusCode))!;
            _errorType = info.GetString("ErrorType")!;
            _errorMessage = info.GetString("ErrorMessage")!;
        }

        public HttpStatusCode ErrorCode { get { return _errorCode; } }
        public string ErrorType { get { return _errorType; } }
        public string ErrorMessage { get { return _errorMessage; } }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ErrorType", _errorType);
            info.AddValue("ErrorMessage", _errorMessage);
            info.AddValue("ErrorCode", _errorCode, typeof(HttpStatusCode));

            // MUST call through to the base class to let it save its own state
            base.GetObjectData(info, context);
        }
    }
}
