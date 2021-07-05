using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class PublisherModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is Required")]
        [MaxLength(20, ErrorMessage = "First Name should be less than 20.")]
        [MinLength(5, ErrorMessage = "First Name should be greater than 5.")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Last Name is Required")]
        [MaxLength(20, ErrorMessage = "Last Name should be less than 20.")]
        [MinLength(5, ErrorMessage = "Last Name should be greater than 5.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "State Name is Required")]
        [MaxLength(15, ErrorMessage = "Publisher Name should be less than 20.")]
        [MinLength(5, ErrorMessage = "Publisher Name should be greater than 5.")]

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
    }
}
