using System.Collections.Generic;
using wie_doet_de_afwas.Models;

namespace wie_doet_de_afwas.ViewModels
{
    public class ListTaskGroupsViewModel : List<SingleTaskGroupViewModel>
    {
        public ListTaskGroupsViewModel(IEnumerable<TaskGroup> taskGroups)
        {
            foreach (TaskGroup taskGroup in taskGroups)
            {
                Add(new SingleTaskGroupViewModel(taskGroup));
            }
        }
    }

    public class SingleTaskGroupViewModel
    {
        public SingleTaskGroupViewModel(TaskGroup taskGroup)
        {
            this.Id = taskGroup.Id;
            this.Name = taskGroup.Name;
        }

        public string Id { get; set; }
        
        public string Name { get; set; }
    }
}