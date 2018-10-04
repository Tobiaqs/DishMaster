using wie_doet_de_afwas.Models;

namespace wie_doet_de_afwas.ViewModels
{
    public class ListGroupsViewModel
    {
        public ListGroupsViewModel(Group group)
        {
            this.Id = group.Id;
            this.Name = group.Name;
        }

        public string Id { get; set; }
        
        public string Name { get; set; }
    }
}