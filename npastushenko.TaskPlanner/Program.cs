using System;
using System.Collections.Generic;
using System.Text;
using Domain.Model;
using Domain.Model.Enums;
using DataAccess;
using DataAccess.Abstractions;

internal static class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.Unicode;
        Console.WriteLine("=== Simple Task Planner ===");

        IWorkItemsRepository repo = new FileWorkItemsRepository();

        while (true)
        {
            Console.WriteLine("\n===== МЕНЮ =====");
            Console.WriteLine("1. Показати задачі");
            Console.WriteLine("2. Додати задачу");
            Console.WriteLine("3. Видалити задачу");
            Console.WriteLine("4. Вийти");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowTasks(repo);
                    break;

                case "2":
                    AddTask(repo);
                    break;

                case "3":
                    DeleteTask(repo);
                    break;

                case "4":
                    return;

                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }
        }
    }

    // 🔥 1. Показати задачі
    private static void ShowTasks(IWorkItemsRepository repo)
    {
        var all = repo.GetAll();

        if (all.Length == 0)
        {
            Console.WriteLine("Список порожній.");
            return;
        }

        Console.WriteLine("\n=== Список задач ===");

        var planner = new SimpleTaskPlanner();
        var sorted = planner.CreatePlan(all);

        foreach (var item in sorted)
            Console.WriteLine(item);
    }

    // 🔥 2. Додати задачу (твій код майже без змін)
    private static void AddTask(IWorkItemsRepository repo)
    {
        Console.WriteLine("\n=== Створення нового завдання ===");

        while (true)
        {
            Console.Write("Назва (Title): ");
            string title = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(title))
                break;

            Console.Write("Опис (Description): ");
            string description = Console.ReadLine();

            Console.Write("Дата створення (yyyy-MM-dd): ");
            DateTime creationDate = DateTime.Parse(Console.ReadLine());

            Console.Write("Кінцева дата (yyyy-MM-dd): ");
            DateTime dueDate = DateTime.Parse(Console.ReadLine());

            Console.Write("Пріоритет (Low, Medium, High): ");
            Priority priority = Enum.Parse<Priority>(Console.ReadLine(), true);

            Console.Write("Складність (None, Minutes, Hours, Days, Weeks): ");
            Complexity complexity = Enum.Parse<Complexity>(Console.ReadLine(), true);

            var item = new WorkItem
            {
                Title = title,
                Description = description,
                CreationDate = creationDate,
                DueDate = dueDate,
                Priority = priority,
                Complexity = complexity,
                IsCompleted = false
            };

            Guid newId = repo.Add(item);
            repo.SaveChanges();

            Console.WriteLine($"Завдання додано! ID = {newId}\n");

            Console.Write("Додати ще? (y/n): ");
            if (Console.ReadLine().ToLower() != "y")
                break;
        }
    }

    // 🔥 3. Видалити задачу за ID
    private static void DeleteTask(IWorkItemsRepository repo)
    {
        var all = repo.GetAll();

        if (all.Length == 0)
        {
            Console.WriteLine("Немає задач для видалення.");
            return;
        }

        Console.WriteLine("\n=== Видалення задачі ===");
        Console.WriteLine("Список задач:");

        foreach (var item in all)
            Console.WriteLine($"{item.Id} — {item.Title}");

        Console.Write("\nВведіть ID задачі для видалення: ");
        string idString = Console.ReadLine();

        if (!Guid.TryParse(idString, out Guid id))
        {
            Console.WriteLine("Невірний формат ID.");
            return;
        }

        bool result = repo.Remove(id);

        if (result)
        {
            repo.SaveChanges();
            Console.WriteLine("Задачу видалено!");
        }
        else
        {
            Console.WriteLine("Задачі з таким ID не знайдено.");
        }
    }
}
