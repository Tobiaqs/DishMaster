using System.ComponentModel.DataAnnotations;
using DishMaster.Annotations;
using DishMaster.Logic;

namespace DishMaster.ViewModels
{
    public class UpdateGroupViewModel
    {
        [IsGuid]
        public string GroupId { get; set; }

        [IsValidName]
        public string Name { get; set; }
    }
}