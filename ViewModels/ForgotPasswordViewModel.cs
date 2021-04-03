using System.ComponentModel.DataAnnotations;

namespace DishMaster.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        public string Email { get; set; }
    }
}