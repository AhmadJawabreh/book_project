using System;
using System.Collections.Generic;

namespace Resources
{

    public class BookResource
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        public PublisherResource publisher { get; set; }

        public List<AuthorResource> Authors { get; set; }
    }
}
