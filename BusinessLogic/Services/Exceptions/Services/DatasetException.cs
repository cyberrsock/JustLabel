using System;

namespace JustLabel.Exceptions;

public class FailedDatasetCreationException : BaseException
{
    public FailedDatasetCreationException() : base() { }
    public FailedDatasetCreationException(string message) : base(message) { }
    public FailedDatasetCreationException(string message, Exception innerException) : base(message, innerException) { }
    public FailedDatasetCreationException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}

public class DatasetNotExitedException : BaseException
{
    public DatasetNotExitedException() : base("The dataset does not exist") { }
    public DatasetNotExitedException(string message) : base(message) { }
    public DatasetNotExitedException(string message, Exception innerException) : base(message, innerException) { }
    public DatasetNotExitedException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}
