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
            Console.WriteLine("\n===== MENU =====");
            Console.WriteLine("[A] Add work item");
            Console.WriteLine("[B] Build a plan (show sorted)");
            Console.WriteLine("[M] Mark work item as completed");
            Console.WriteLine("[R] Remove a work item");
            Console.WriteLine("[Q] Quit");
            Console.Write("Choose: ");

            string choice = Console.ReadLine().Trim().ToUpper();

            switch (choice)
            {
                case "A":
                    AddTask(repo);
                    break;

                case "B":
                    BuildPlan(repo);
                    break;

                case "M":
                    MarkCompleted(repo);
                    break;

                case "R":
                    DeleteTask(repo);
                    break;

                case "Q":
                    return;

                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }
        }
    }

    // 🔥 A — Add work item
    private static void AddTask(IWorkItemsRepository repo)
    {
        Console.WriteLine("\n=== Create new work item ===");

        Console.Write("Title: ");
        string title = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Canceled.");
            return;
        }

        Console.Write("Description: ");
        string description = Console.ReadLine();

        Console.Write("Creation date (yyyy-MM-dd): ");
        DateTime creationDate = DateTime.Parse(Console.ReadLine());

        Console.Write("Due date (yyyy-MM-dd): ");
        DateTime dueDate = DateTime.Parse(Console.ReadLine());

        Console.Write("Priority (Low, Medium, High): ");
        Priority priority = Enum.Parse<Priority>(Console.ReadLine(), true);

        Console.Write("Complexity (None, Minutes, Hours, Days, Weeks): ");
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

        Guid id = repo.Add(item);
        repo.SaveChanges();

        Console.WriteLine($"Added! ID = {id}");
    }

    // 🔥 B — Build a plan (sort items)
    private static void BuildPlan(IWorkItemsRepository repo)
    {
        var all = repo.GetAll();

        if (all.Length == 0)
        {
            Console.WriteLine("No items found.");
            return;
        }

        var planner = new SimpleTaskPlanner();
        var sorted = planner.CreatePlan(all);

        Console.WriteLine("\n=== Sorted plan ===");
        foreach (var item in sorted)
            Console.WriteLine(item);
    }

    // 🔥 M — Mark item as completed
    private static void MarkCompleted(IWorkItemsRepository repo)
    {
        var all = repo.GetAll();

        if (all.Length == 0)
        {
            Console.WriteLine("No items available.");
            return;
        }

        Console.WriteLine("\n=== Mark as completed ===");
        foreach (var item in all)
            Console.WriteLine($"{item.Id} — {item.Title} (Completed: {item.IsCompleted})");

        Console.Write("\nEnter ID: ");
        string idStr = Console.ReadLine();

        if (!Guid.TryParse(idStr, out Guid id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        var itemToUpdate = repo.Get(id);
        if (itemToUpdate == null)
        {
            Console.WriteLine("Item not found.");
            return;
        }

        itemToUpdate.IsCompleted = true;
        repo.Update(itemToUpdate);
        repo.SaveChanges();

        Console.WriteLine("Marked as completed!");
    }

    // 🔥 R — Remove work item
    private static void DeleteTask(IWorkItemsRepository repo)
    {
        var all = repo.GetAll();

        if (all.Length == 0)
        {
            Console.WriteLine("No items to remove.");
            return;
        }

        Console.WriteLine("\n=== Remove work item ===");
        foreach (var item in all)
            Console.WriteLine($"{item.Id} — {item.Title}");

        Console.Write("\nEnter ID: ");
        string idStr = Console.ReadLine();

        if (!Guid.TryParse(idStr, out Guid id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        bool ok = repo.Remove(id);

        if (ok)
        {
            repo.SaveChanges();
            Console.WriteLine("Item removed!");
        }
        else
        {
            Console.WriteLine("Item not found.");
        }
    }
}
