using System.ComponentModel.DataAnnotations;
using wie_doet_de_afwas.Annotations;
using wie_doet_de_afwas.Logic;

namespace wie_doet_de_afwas.ViewModels
{
    public class ListTaskGroupRecordsViewModel
    {
        [IsGuid]
        public string TaskGroupId { get; set; }

        public int Count { get; set; }

        public int Offset { get; set; }
        
        public bool Superficial { get; set; }
    }
}