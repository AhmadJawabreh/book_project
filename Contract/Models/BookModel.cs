using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class BookModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Book Name is Required")]
        [MaxLength(35, ErrorMessage = "Book Name should be less than 35")]
        [MinLength(5, ErrorMessage = "Book Name should be greater than 5")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Release Date is Required")]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "Please select a publisher")]
        public int PublisherId { get; set; }

        public List<long> AuthorIds { get; set; }
    }
}
