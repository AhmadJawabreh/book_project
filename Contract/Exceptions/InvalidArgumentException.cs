using System;

namespace Contract.Exceptions
{
    public class InvalidArgumentException : Exception
    {
        public InvalidArgumentException(string title) : base(title)
        {
        }
    }
}
