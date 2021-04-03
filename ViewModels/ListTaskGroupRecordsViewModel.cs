using DishMaster.Annotations;

namespace DishMaster.ViewModels
{
    public class ListTaskGroupRecordsViewModel
    {
        [IsGuid]
        public string TaskGroupId { get; set; }

        public int Count { get; set; }

        public int Offset { get; set; }
        
        public bool Superficial { get; set; }
    }
}