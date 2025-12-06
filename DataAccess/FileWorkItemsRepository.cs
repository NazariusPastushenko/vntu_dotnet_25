using System;
using System.Collections.Generic;
using System.IO;
using DataAccess.Abstractions;
using Domain.Model;
using Newtonsoft.Json;


namespace DataAccess;

public class FileWorkItemsRepository : IWorkItemsRepository
{
    private const string FILE_NAME = "work-items.json";
    private readonly Dictionary<Guid, WorkItem> _items;

    public FileWorkItemsRepository()
    {
        if (File.Exists(FILE_NAME) && new FileInfo(FILE_NAME).Length > 0)
        {
            string json = File.ReadAllText(FILE_NAME);

            var array = JsonConvert.DeserializeObject<WorkItem[]>(json);

            _items = new Dictionary<Guid, WorkItem>();

            if (array != null)
                foreach (var item in array)
                    _items[item.Id] = item;
        }
        else
        {
            _items = new Dictionary<Guid, WorkItem>();
        }
    }

    public Guid Add(WorkItem workItem)
    {
        // створюємо копію
        var clone = workItem.Clone();

        // генеруємо новий Id
        Guid newId = Guid.NewGuid();
        clone.Id = newId;

        _items[newId] = clone;

        return newId;
    }

    public WorkItem Get(Guid id)
    {
        return _items.TryGetValue(id, out var item) ? item.Clone() : null;
    }

    public WorkItem[] GetAll()
    {
        var array = new List<WorkItem>();
        foreach (var item in _items.Values)
        {
            if (item.IsCompleted) continue;
            array.Add(item.Clone());
        }

        return array.ToArray();
    }

    public bool Update(WorkItem workItem)
    {
        if (!_items.ContainsKey(workItem.Id))
            return false;

        _items[workItem.Id] = workItem.Clone();
        return true;
    }

    public bool Remove(Guid id)
    {
        return _items.Remove(id);
    }

    public void SaveChanges()
    {
        var array = new List<WorkItem>(_items.Values);
        string json = JsonConvert.SerializeObject(array, Formatting.Indented);
        File.WriteAllText(FILE_NAME, json);
    }
}
