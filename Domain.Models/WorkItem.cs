using Domain.Model.Enums;

namespace Domain.Model;

public class WorkItem
{
    public DateTime CreationDate { get; set; }
    public DateTime DueDate { get; set; }
    public Priority Priority { get; set; }
    public Complexity Complexity { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public Guid Id { get; set; }

    public WorkItem Clone()
    {
        return new WorkItem
        {
            Id = this.Id,
            Title = this.Title,
            Description = this.Description,
            CreationDate = this.CreationDate,
            DueDate = this.DueDate,
            Priority = this.Priority,
            Complexity = this.Complexity,
            IsCompleted = this.IsCompleted
        };
    }

    public override string ToString()
    {
        return $"{Title}: due {DueDate:dd.MM.yyyy}, {Priority.ToString().ToLower()} priority. Complete: {IsCompleted}";
    }
}