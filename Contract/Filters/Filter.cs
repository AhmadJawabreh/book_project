using Contract.Filters;

namespace Filters
{
    public class Filter : Pagination
    {
        public string BookName { get; set; }

        public bool Publisher { get; set; }

        public bool Authors { get; set; }
    }
}
