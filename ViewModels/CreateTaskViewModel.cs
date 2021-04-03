using System.ComponentModel.DataAnnotations;
using DishMaster.Annotations;
using DishMaster.Logic;

namespace DishMaster.ViewModels
{
    public class CreateTaskViewModel
    {
        [IsValidName]
        public string Name { get; set; }

        [IsValidBounty]
        public int Bounty { get; set; }

        public bool IsNeutral { get; set; }

        [IsGuid]
        public string TaskGroupId { get; set; }
    }
}