using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface ITaskService
    {
        void CreateTask(string title, string description, string assignee);
        List<TaskItem> GetTasksForUser(string username);
        void ChangeStatus(int taskId, Models.TaskStatus newStatus);
    }
}