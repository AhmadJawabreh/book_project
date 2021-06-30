using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{

    [Table("Publisher", Schema = "ExternalResources")]
    public class Publisher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Required(ErrorMessage = "Publisher Name is required.")]
        [MaxLength(20, ErrorMessage = "Publisher Name should be less than 20.")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Publisher Name is required.")]
        [MaxLength(20, ErrorMessage = "Publisher Name should be less than 20.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Publisher Address is required")]
        public string Address { get; set; }

        [Phone]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
