using System;

namespace Contract.Exceptions
{
    public class DubplicateDataException : Exception
    {
        public DubplicateDataException(string title) : base(title)
        {
        }
    }
}
