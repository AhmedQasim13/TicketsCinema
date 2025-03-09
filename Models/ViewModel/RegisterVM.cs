using System.ComponentModel.DataAnnotations;

namespace TicketsCinema.Models.ViewModel
{
    public class RegisterVM
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Fullname { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]//compare the password with the confirm password == [compare (nameof(password))]
        public string ConfirmPassword { get; set; }

    }
}
