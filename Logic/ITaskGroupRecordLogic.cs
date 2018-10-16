using wie_doet_de_afwas.Models;

namespace wie_doet_de_afwas.Logic
{
    public interface ITaskGroupRecordLogic
    {
        bool FillTaskGroupRecord(WDDAContext wDDAContext, TaskGroupRecord taskGroupRecord, CreateTaskGroupRecordViewModel createTaskGroupRecordViewModel);
    }
}