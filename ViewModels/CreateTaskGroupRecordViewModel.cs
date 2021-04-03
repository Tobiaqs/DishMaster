using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DishMaster.Annotations;

namespace DishMaster
{
    public class CreateTaskGroupRecordViewModel
    {
        [IsGuid]
        public string TaskGroupId { get; set; }

        [ContainsGuids(1, false)]
        public IEnumerable<string> PresentGroupMembersIds { get; set; }
    }
}