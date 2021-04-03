using DishMaster.Models;

namespace DishMaster.ViewModels
{
    // View model used for outputting only
    public class ListGroupsViewModel
    {
        public ListGroupsViewModel(Group group)
        {
            this.Id = group.Id;
            this.Name = group.Name;
        }

        public string Id { get; }
        public string Name { get; }
    }
}