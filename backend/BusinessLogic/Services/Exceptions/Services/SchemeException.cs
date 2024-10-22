using System;

namespace JustLabel.Exceptions;

public class SchemeException : BaseException
{
    public SchemeException() : base() { }
    public SchemeException(string message) : base(message) { }
    public SchemeException(string message, Exception innerException) : base(message, innerException) { }
    public SchemeException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}
