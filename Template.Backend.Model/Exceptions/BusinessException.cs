using System;
using System.Runtime.Serialization;

namespace Template.Backend.Model.Exceptions
{
    [Serializable]
    public class BusinessException : Exception
    {
        public BusinessException() : base()
        {
        }
        public BusinessException(string message) : base(message)
        {
        }
        public BusinessException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BusinessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class ModelStateException : BusinessException
    {
        public ModelStateException(string message) : base(message)
        {
        }
    }

    [Serializable]
    public class IdNotFoundException : BusinessException
    {
        public IdNotFoundException(string message) : base(message)
        {
        }
    }

    [Serializable]
    public class NoElementFoundException : BusinessException
    {
        public NoElementFoundException(string message) : base(message)
        {
        }
    }

    [Serializable]
    public class CanNotBeDeletedException : BusinessException
    {
        public CanNotBeDeletedException(string message) : base(message)
        {
        }
    }

    [Serializable]
    public class ConfigFileNotFoundException : BusinessException
    {
        public ConfigFileNotFoundException(string message) : base(message)
        {
        }
    }

    [Serializable]
    public class ApiKeyNotFoundException : BusinessException
    {
        public ApiKeyNotFoundException(string message) : base(message)
        {
        }
    }

    [Serializable]
    public class DateTimeFormatException : BusinessException
    {
        public DateTimeFormatException(string message) : base(message)
        {
        }
    }

    [Serializable]
    public class DatabaseUpdateException : BusinessException
    {
        public DatabaseUpdateException() : base()
        {
        }
        public DatabaseUpdateException(string message) : base(message)
        {
        }
    }

    [Serializable]
    public class BadRequestException : BusinessException
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }

    [Serializable]
    public class UnauthorizedException : BusinessException
    {
        public UnauthorizedException(string message) : base(message)
        {
        }
    }

    [Serializable]
    public class InvalidGrantException : BusinessException
    {
        public InvalidGrantException() : base()
        {
        }
    }

    [Serializable]
    public class ApiServerException : BusinessException
    {
        public ApiServerException(string message) : base(message)
        {
        }
    }

    [Serializable]
    public class TaskCanceledBusinessException : BusinessException
    {
        public TaskCanceledBusinessException(string message) : base(message)
        {
        }
    }
}