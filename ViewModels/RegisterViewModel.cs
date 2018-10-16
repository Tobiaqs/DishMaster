using wie_doet_de_afwas.Annotations;
using wie_doet_de_afwas.Logic;

namespace wie_doet_de_afwas.ViewModels
{
    public class RegisterViewModel : LoginViewModel
    {
        [IsValidName]
        public string FullName { get; set; }
    }
}