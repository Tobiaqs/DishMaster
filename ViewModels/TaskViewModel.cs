using System.Collections.Generic;
using System.Linq;
using wie_doet_de_afwas.Models;

namespace wie_doet_de_afwas.ViewModels
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