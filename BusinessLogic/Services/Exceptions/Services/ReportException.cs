using System;

namespace JustLabel.Exceptions;

public class ReportException : BaseException
{
    public ReportException() : base() { }
    public ReportException(string message) : base(message) { }
    public ReportException(string message, Exception innerException) : base(message, innerException) { }
    public ReportException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}
