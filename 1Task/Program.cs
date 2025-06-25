using Interfaces;
using Services;
using System;

class Program
{
    static void Main()
    {
        var auth = new AuthenticationService(new JsonUserStorage("users\\users.json"));
        var taskService = new TaskService();

        Console.Write("Логин: ");
        string login = Console.ReadLine();
        Console.Write("Пароль: ");
        string password = PasswordUtils.ReadPassword(_ => true);

        if (!auth.Login(login, password, out var user))
        {
            Console.WriteLine("Неверный логин или пароль.");
            Console.WriteLine("Нажмите Enter для выхода...");
            Console.ReadLine();
            return;
        }

        if (user.Role == "Admin")
        {
            Console.WriteLine("\nВы вошли как администратор.");
            while (true)
            {
                
                Console.WriteLine("1. Зарегистрировать пользователя");
                Console.WriteLine("2. Создать и назначить задачу");
                Console.WriteLine("3. Список сотруников");
                Console.WriteLine("4. Выйти из программы");

                var ch = Console.ReadLine();

                if (ch == "1")
                {
                    Console.Write("Новый логин: ");
                    var newLogin = Console.ReadLine();
                    Console.Write("Пароль: ");
                    var newPassword = PasswordUtils.ReadPassword(char.IsLetterOrDigit);
                    auth.Register(newLogin, newPassword, "User");
                }
                else if (ch == "2")
                {
                    Console.Write("Название задачи: ");
                    string title = Console.ReadLine();
                    Console.Write("Описание: ");
                    string desc = Console.ReadLine();
                    Console.Write("Назначить на (логин): ");
                    string to = Console.ReadLine();
                    taskService.CreateTask(title, desc, to);
                }
                else if (ch == "3")
                {
                    Console.WriteLine("\nСписок сотрудников:");
                    var usersList = auth.GetAllUsers();
                    foreach (var u in usersList)
                    {
                        Console.WriteLine($"Логин: {u.Username} | Роль: {u.Role}");
                    }
                }
                else if (ch == "4")
                {
                    Console.WriteLine("Выход...");
                    break;
                }
                else
                {
                    Console.WriteLine("Неизвестная команда.");
                }
            }
        }
        else if (user.Role == "User")
        {
            Console.WriteLine($"\nВы вошли как пользователь - {login}.");
            while (true)
            {
                
                Console.WriteLine("1. Посмотреть задачи");
                Console.WriteLine("2. Изменить статус задачи");
                Console.WriteLine("3. Выйти из программы");
                var ch = Console.ReadLine();

                if (ch == "1")
                {
                    Console.WriteLine("Ваши задачи:");
                    var tasks = taskService.GetTasksForUser(user.Username);
                    foreach (var task in tasks)
                    {
                        Console.WriteLine($"{task.Id} | {task.Title} | {task.Description} [{task.Status}]");
                    }
                }

                else if(ch == "2")
                {
                    Console.Write("Введите ID задачи для смены статуса: ");
                    if (int.TryParse(Console.ReadLine(), out var taskId))
                    {
                        Console.Write("Введите новый статус (ToDo, InProgress, Done): ");
                        var newStatus = Enum.Parse<Models.TaskStatus>(Console.ReadLine(), true);
                        taskService.ChangeStatus(taskId, newStatus);
                    }
                }
                else if (ch == "3")
                {
                    Console.WriteLine("Выход...");
                    break;
                }
                else
                {
                    Console.WriteLine("Неизвестная команда.");
                }
            }
        }
    }
}
