using System.ComponentModel.DataAnnotations;
using DishMaster.Logic;

namespace DishMaster.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}