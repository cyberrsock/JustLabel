using System;

namespace JustLabel.Exceptions;

public class LabelException : BaseException
{
    public LabelException() : base() { }
    public LabelException(string message) : base(message) { }
    public LabelException(string message, Exception innerException) : base(message, innerException) { }
    public LabelException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}
