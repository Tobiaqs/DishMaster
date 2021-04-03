using System.ComponentModel.DataAnnotations;
using DishMaster.Annotations;
using DishMaster.Logic;

namespace DishMaster.ViewModels
{
    public class CreateTaskGroupViewModel
    {
        [IsValidName]
        public string Name { get; set; }

        [IsGuid]
        public string GroupId { get; set; }
    }
}