using DishMaster.Annotations;

namespace DishMaster.ViewModels
{
    public class UnassignTaskViewModel
    {
        [IsGuid]
        public string TaskId { get; set; }

        [IsGuid]
        public string TaskGroupRecordId { get; set; }
    }
}