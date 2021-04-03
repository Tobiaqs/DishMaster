using System.ComponentModel.DataAnnotations;
using DishMaster.Annotations;
using DishMaster.Logic;

namespace DishMaster.ViewModels
{
    public class UpdateTaskViewModel
    {
        [IsGuid]
        public string TaskId { get; set; }

        [IsValidName]
        public string Name { get; set; }

        [IsValidBounty]
        public int Bounty { get; set; }
    }
}