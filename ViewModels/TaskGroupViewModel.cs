using System.Collections.Generic;
using System.Linq;
using DishMaster.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DishMaster.ViewModels
{
    // View model used for outputting only
    public class TaskGroupViewModel
    {
        public TaskGroupViewModel(TaskGroup taskGroup)
        {
            this.Id = taskGroup.Id;
            this.Tasks = taskGroup.Tasks
                .OrderBy(task => task.Name.ToLower())
                .Select(task => new TaskViewModel(task));
            this.Name = taskGroup.Name;
        }

        public string Id { get; }
        public IEnumerable<TaskViewModel> Tasks { get; }
        public string Name { get; }
    }
}