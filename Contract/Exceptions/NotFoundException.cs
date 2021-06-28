using System;

namespace Contract.Exceptions
{

    public class NotFoundException : Exception
    {
        public NotFoundException(string title) : base(title)
        {
        }
    }
}
