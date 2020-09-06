using System.ComponentModel.DataAnnotations;
using wie_doet_de_afwas.Annotations;
using wie_doet_de_afwas.Logic;

namespace wie_doet_de_afwas.ViewModels
{
    public class ResetPasswordViewModel : LoginViewModel
    {
        [Required]
        public string ResetToken { get; set; }
    }
}