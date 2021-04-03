using System.ComponentModel.DataAnnotations;
using DishMaster.Annotations;
using DishMaster.Logic;

namespace DishMaster.ViewModels
{
    public class UpdateTaskGroupViewModel
    {        
        [IsGuid]
        public string TaskGroupId { get; set; }

        [IsValidName]
        public string Name { get; set; }
    }
}