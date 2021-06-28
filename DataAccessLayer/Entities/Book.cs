using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [MaxLength(35, ErrorMessage = "Book Name should be greater than 35")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Release Date is Required")]
        public DateTime ReleaseDate { get; set; }

        public long? PublisherId { get; set; }

    }
}
