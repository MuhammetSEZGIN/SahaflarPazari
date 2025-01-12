using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace SahaflarPazari.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name is required")]
        [StringLength(255, ErrorMessage = "User Name cannot be longer than 255 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(255, ErrorMessage = "First Name cannot be longer than 255 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(255, ErrorMessage = "Last Name cannot be longer than 255 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid Phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email address")]
        public string Email { get; set; }

        // For password, we can add more feature.
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
        public string ConfirmPassword { get; set; }


    }
}