using System;
using System.Collections.Generic;
using System.Text;
using Domain.Model;
using Domain.Model.Enums;


internal static class Program
{
    public static void Main(string[] args)
    {
        var workItems = new List<WorkItem>();

        Console.OutputEncoding = Encoding.Unicode;
        Console.WriteLine("=== Simple Task Planner ===");
        Console.WriteLine("Введіть завдання (порожня назва = завершення)\n");

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

            workItems.Add(new WorkItem
            {
                Title = title,
                Description = description,
                CreationDate = creationDate,
                DueDate = dueDate,
                Priority = priority,
                Complexity = complexity,
                IsCompleted = false
            });

            Console.WriteLine("Завдання додано!\n");
        }

        if (workItems.Count == 0)
        {
            Console.WriteLine("Не додано жодного завдання.");
            return;
        }

        var planner = new SimpleTaskPlanner();
        var sorted = planner.CreatePlan(workItems.ToArray());

        Console.WriteLine("\n=== Відсортований список ===");
        foreach (var item in sorted)
            Console.WriteLine(item);

        Console.WriteLine("\nНатисніть будь-яку клавішу для завершення...");
        Console.ReadKey();
    }
}
