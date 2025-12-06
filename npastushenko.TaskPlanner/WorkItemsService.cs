using DataAccess.Abstractions;
using Domain.Model;

namespace npastushenko.TaskPlanner;

public class WorkItemsService
{
    private readonly IWorkItemsRepository _repo;

    public WorkItemsService(IWorkItemsRepository repo)
    {
        _repo = repo;
    }

    public Guid Add(WorkItem item)
    {
        var id = _repo.Add(item);
        _repo.SaveChanges();
        return id;
    }

    public bool MarkCompleted(Guid id)
    {
        var item = _repo.Get(id);
        if (item == null) return false;

        item.IsCompleted = true;
        _repo.Update(item);
        _repo.SaveChanges();
        return true;
    }

    public bool Remove(Guid id)
    {
        bool ok = _repo.Remove(id);
        if (ok)
            _repo.SaveChanges();
        return ok;
    }
}