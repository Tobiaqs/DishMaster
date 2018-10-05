using System.Collections.Generic;
using System.Linq;
using wie_doet_de_afwas.Models;

namespace wie_doet_de_afwas.ViewModels
{
    // View model used for outputting only
    public class TaskGroupViewModel
    {
        public TaskGroupViewModel(TaskGroup taskGroup)
        {
            this.Id = taskGroup.Id;
            this.TaskIds = taskGroup.Tasks.Select<Task, string>((t) => t.Id);
        }

        public readonly string Id;
        public readonly IEnumerable<string> TaskIds;
    }
}