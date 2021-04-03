using System.ComponentModel.DataAnnotations;
using DishMaster.Annotations;

namespace DishMaster.ViewModels
{
    public class AddAnonymousGroupMemberViewModel
    {
        [IsValidName]
        public string AnonymousName { get; set; }

        [IsGuid]
        public string GroupId { get; set; }
    }
}