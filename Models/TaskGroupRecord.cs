using wie_doet_de_afwas.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace wie_doet_de_afwas.Models
{
    public class TaskGroupRecord
    {
        public TaskGroupRecord()
        {
            this.TaskIdToGroupMemberIdMap = new Dictionary<string, string>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public TaskGroup TaskGroup { get; set; }

        [Required]
        public System.DateTime Date { get; set; }

        [GuidToGuidMap]
        public IDictionary<string, string> TaskIdToGroupMemberIdMap { get; set; }

        public bool Finalized { get; set; }
    }
}