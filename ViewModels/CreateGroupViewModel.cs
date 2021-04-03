using System.ComponentModel.DataAnnotations;
using DishMaster.Annotations;

namespace DishMaster.ViewModels
{
    public class CreateGroupViewModel
    {
        [IsValidName]
        public string Name { get; set; }
    }
}