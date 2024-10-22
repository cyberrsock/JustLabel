using System;

namespace JustLabel.Exceptions;

public class ImageException : BaseException
{
    public ImageException() : base() { }
    public ImageException(string message) : base(message) { }
    public ImageException(string message, Exception innerException) : base(message, innerException) { }
    public ImageException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}
