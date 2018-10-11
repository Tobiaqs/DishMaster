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

        public readonly string Id;
        public readonly string Name;
        public readonly int Bounty;
        public readonly bool IsNeutral;
    }
}