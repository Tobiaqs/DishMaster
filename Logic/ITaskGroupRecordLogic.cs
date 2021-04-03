using DishMaster.Models;
using DishMaster.Data;

namespace DishMaster.Logic
{
    public interface ITaskGroupRecordLogic
    {
        bool FillTaskGroupRecord(DMContext dMContext, TaskGroupRecord taskGroupRecord, CreateTaskGroupRecordViewModel createTaskGroupRecordViewModel);
    }
}