using System.Collections.Generic;
using System.Linq;
using DishMaster.Models;

namespace DishMaster.ViewModels
{
    // View model used for outputting only
    public class TaskViewModel
    {
        public TaskViewModel(Task task)
        {
            this.Id = task.Id;
            this.Name = task.Name;
            this.Bounty = task.Bounty;
            this.IsNeutral = task.IsNeutral;
        }

        public string Id { get; }
        public string Name { get; }
        public int Bounty { get; }
        public bool IsNeutral { get; }
    }
}