using System.Collections.Generic;
using System.Linq;
using wie_doet_de_afwas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace wie_doet_de_afwas.ViewModels
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

        public readonly string Id;
        public readonly IEnumerable<TaskViewModel> Tasks;
        public readonly string Name;
    }
}