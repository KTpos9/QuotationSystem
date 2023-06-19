using System;

namespace Zero.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string errorMessage) : base(errorMessage)
        {
        }
    }
}