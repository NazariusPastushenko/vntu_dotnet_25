using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Model;

public class SimpleTaskPlanner
{
    public WorkItem[] CreatePlan(WorkItem[] items)
    {
        var itemsAsList = items.ToList();
        itemsAsList.Sort(CompareWorkItems);
        return itemsAsList.ToArray();
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