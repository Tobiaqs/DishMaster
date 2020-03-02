using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using wie_doet_de_afwas.Models;

namespace wie_doet_de_afwas.ViewModels
{
    // View model used for outputting only
    public class SuperficialTaskGroupRecordViewModel
    {
        public SuperficialTaskGroupRecordViewModel(TaskGroupRecord taskGroupRecord)
        {
            this.Id = taskGroupRecord.Id;
            this.Date = taskGroupRecord.Date;
            this.Finalized = taskGroupRecord.Finalized;
        }

        public string Id { get; }

        public System.DateTime Date { get ; }

        public bool Finalized { get; }
    }
}