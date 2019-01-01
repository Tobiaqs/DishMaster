using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using wie_doet_de_afwas.Annotations;
using wie_doet_de_afwas.Logic;
using wie_doet_de_afwas.Models;

namespace wie_doet_de_afwas.ViewModels
{
    // View model used for outputting only
    public class TaskGroupRecordsViewModel : List<SuperficialTaskGroupRecordViewModel>
    {
        public TaskGroupRecordsViewModel(IEnumerable<TaskGroupRecord> collection) : this(collection, false)
        { }

        public TaskGroupRecordsViewModel(IEnumerable<TaskGroupRecord> collection, bool superficial) : base()
        {
            if (superficial)
            {
                this.AddRange(collection.Select(tgr => new SuperficialTaskGroupRecordViewModel(tgr)));
            }
            else
            {
                this.AddRange(collection.Select(tgr => new TaskGroupRecordViewModel(tgr)));
            }
        }
    }
}