using System.Collections.Generic;
using DishMaster.Models;

namespace DishMaster.ViewModels
{
    // View model used for outputting only
    public class ListTaskGroupsViewModel : List<SingleTaskGroupViewModel>
    {
        public ListTaskGroupsViewModel(IEnumerable<TaskGroup> taskGroups)
        {
            foreach (TaskGroup taskGroup in taskGroups)
            {
                Add(new SingleTaskGroupViewModel(taskGroup));
            }
        }
    }

    // View model used for outputting only
    public class SingleTaskGroupViewModel
    {
        public SingleTaskGroupViewModel(TaskGroup taskGroup)
        {
            this.Id = taskGroup.Id;
            this.Name = taskGroup.Name;
        }

        public string Id { get; }
        
        public string Name { get; }
    }
}