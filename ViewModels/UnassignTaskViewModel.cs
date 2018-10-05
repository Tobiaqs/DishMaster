using wie_doet_de_afwas.Annotations;

namespace wie_doet_de_afwas.ViewModels
{
    public class UnassignTaskViewModel
    {
        [IsGuid]
        public string TaskId { get; set; }

        [IsGuid]
        public string TaskGroupRecordId { get; set; }
    }
}