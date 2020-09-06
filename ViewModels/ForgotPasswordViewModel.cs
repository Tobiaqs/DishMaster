using System.ComponentModel.DataAnnotations;

namespace wie_doet_de_afwas.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        public string Email { get; set; }
    }
}