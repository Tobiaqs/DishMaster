using System.ComponentModel.DataAnnotations.Schema;

namespace wie_doet_de_afwas.ViewModels
{
    public class GroupMemberViewModel
    {
        public string PersonId;

        public string GroupId;
        
        public bool Administrator;
    }
}