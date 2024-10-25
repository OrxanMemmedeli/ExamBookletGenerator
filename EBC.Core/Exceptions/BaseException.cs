using System;
using System.Runtime.Serialization;

namespace EBC.Core.Exceptions;

public class BaseException : Exception
{
    public BaseException() : base() { }
    public BaseException(string message) : base(message) { }
    public BaseException(string message, Exception innerException) : base(message, innerException) { }
    protected BaseException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
