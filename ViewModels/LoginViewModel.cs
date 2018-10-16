using System.ComponentModel.DataAnnotations;
using wie_doet_de_afwas.Logic;

namespace wie_doet_de_afwas.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}