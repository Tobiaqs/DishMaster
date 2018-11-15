namespace wie_doet_de_afwas.Models
{
    public class PresentGroupMember
    {
        public string Id { get; set; }
        public GroupMember GroupMember { get; set; }
        public TaskGroupRecord TaskGroupRecord { get; set; }
    }
}