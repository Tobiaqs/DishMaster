using wie_doet_de_afwas.Models;

namespace wie_doet_de_afwas.ViewModels
{
    // View model used for outputting only
    public class ListGroupsViewModel
    {
        public ListGroupsViewModel(Group group)
        {
            this.Id = group.Id;
            this.Name = group.Name;
        }

        public readonly string Id;
        
        public readonly string Name;
    }
}