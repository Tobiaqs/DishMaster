using System.ComponentModel.DataAnnotations;
using DishMaster.Annotations;
using DishMaster.Logic;

namespace DishMaster.ViewModels
{
    public class ResetPasswordViewModel : LoginViewModel
    {
        [Required]
        public string ResetToken { get; set; }
    }
}