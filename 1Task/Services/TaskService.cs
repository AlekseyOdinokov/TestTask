using Interfaces;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TaskService : ITaskService
    {
        private readonly string _taskFile = "tasks.json";

        public List<TaskItem> LoadTasks()
        {
            if (!File.Exists(_taskFile)) return new List<TaskItem>();
            var json = File.ReadAllText(_taskFile);
            return JsonConvert.DeserializeObject<List<TaskItem>>(json) ?? new List<TaskItem>();
        }

        public void SaveTasks(List<TaskItem> tasks)
        {
            var json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
            File.WriteAllText(_taskFile, json);
        }

        public void CreateTask(string title, string description, string assignee)
        {
            var tasks = LoadTasks();

            int newId = tasks.Any() ? tasks.Max(t => t.Id) + 1 : 1;

            var task = new TaskItem
            {
                Id = newId,
                Title = title,
                Description = description,
                AssignedTo = assignee
            };

            tasks.Add(task);
            SaveTasks(tasks);
            Console.WriteLine($"Задача #{newId} создана и назначена.");
        }

        public List<TaskItem> GetTasksForUser(string username)
        {
            return LoadTasks().Where(t => t.AssignedTo == username).ToList();
        }

        public void ChangeStatus(int taskId, Models.TaskStatus newStatus)
        {
            var tasks = LoadTasks();
            var task = tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                task.Status = newStatus;
                SaveTasks(tasks);
                Console.WriteLine("Статус обновлён.");
            }
        }
    }
}
