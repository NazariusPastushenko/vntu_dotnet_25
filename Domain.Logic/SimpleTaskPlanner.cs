using DataAccess.Abstractions;
using Domain.Model;

namespace Domain.Logic;

public class SimpleTaskPlanner
{
    private readonly IWorkItemsRepository _repo;

    public SimpleTaskPlanner(IWorkItemsRepository repo)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
    }

    public WorkItem[] CreatePlan()
    {
        var items = _repo.GetAll();
        
        return items
            .OrderBy(i => i.IsCompleted)
            .ThenBy(i => i.Priority)
            .ThenBy(i => i.DueDate)
            .ToArray();
    }

    private static int CompareWorkItems(WorkItem firstItem, WorkItem secondItem)
    {

        int priorityCompare = secondItem.Priority.CompareTo(firstItem.Priority);
        if (priorityCompare != 0)
            return priorityCompare;


        int dateCompare = firstItem.DueDate.CompareTo(secondItem.DueDate);
        if (dateCompare != 0)
            return dateCompare;

        return string.Compare(firstItem.Title, secondItem.Title, StringComparison.OrdinalIgnoreCase);
    }
}