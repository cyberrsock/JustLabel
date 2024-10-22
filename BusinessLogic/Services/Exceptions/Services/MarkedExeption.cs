using System;

namespace JustLabel.Exceptions;

public class MarkedException : BaseException
{
    public MarkedException() : base() { }
    public MarkedException(string message) : base(message) { }
    public MarkedException(string message, Exception innerException) : base(message, innerException) { }
    public MarkedException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}
