using DishMaster.Annotations;
using DishMaster.Logic;

namespace DishMaster.ViewModels
{
    public class RegisterViewModel : LoginViewModel
    {
        [IsValidName]
        public string FullName { get; set; }
    }
}