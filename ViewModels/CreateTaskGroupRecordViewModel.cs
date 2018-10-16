using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using wie_doet_de_afwas.Annotations;

namespace wie_doet_de_afwas
{
    public class CreateTaskGroupRecordViewModel
    {
        [IsGuid]
        public string TaskGroupId { get; set; }

        [ContainsGuids(1, false)]
        public IEnumerable<string> PresentGroupMembersIds { get; set; }
    }
}