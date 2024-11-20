using System;

namespace JustLabel.Exceptions;

public class FailedRegistrationException : BaseException
{
    public FailedRegistrationException() : base() { }
    public FailedRegistrationException(string message) : base(message) { }
    public FailedRegistrationException(string message, Exception innerException) : base(message, innerException) { }
    public FailedRegistrationException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}

public class FailedLoginException : BaseException
{
    public FailedLoginException() : base() { }
    public FailedLoginException(string message) : base(message) { }
    public FailedLoginException(string message, Exception innerException) : base(message, innerException) { }
    public FailedLoginException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}

public class UserExistsException : BaseException
{
    public UserExistsException() : base("The user already exists") { }
    public UserExistsException(string message) : base(message) { }
    public UserExistsException(string message, Exception innerException) : base(message, innerException) { }
    public UserExistsException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}

public class UserNotExistsException : BaseException
{
    public UserNotExistsException() : base("The user does not exist") { }
    public UserNotExistsException(string message) : base(message) { }
    public UserNotExistsException(string message, Exception innerException) : base(message, innerException) { }
    public UserNotExistsException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}

public class BannedUserException : BaseException
{
    public BannedUserException() : base("The user is banned") { }
    public BannedUserException(string message) : base(message) { }
    public BannedUserException(string message, Exception innerException) : base(message, innerException) { }
    public BannedUserException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}

public class UnbannedUserException : BaseException
{
    public UnbannedUserException() : base("The user is not banned") { }
    public UnbannedUserException(string message) : base(message) { }
    public UnbannedUserException(string message, Exception innerException) : base(message, innerException) { }
    public UnbannedUserException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}

public class AdminUserException : BaseException
{
    public AdminUserException() : base("The user is an administrator") { }
    public AdminUserException(string message) : base(message) { }
    public AdminUserException(string message, Exception innerException) : base(message, innerException) { }
    public AdminUserException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}
