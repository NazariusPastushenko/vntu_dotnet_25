using DataAccess.Abstractions;
using Domain.Logic;
using Domain.Model;
using Domain.Model.Enums;
using Moq;
using npastushenko.TaskPlanner;

namespace xUnit_Test;

public class WorkItemsServiceTests
{
    [Fact]
    public void Add_ShouldCallRepoAdd_AndSaveChanges()
    {
        var repo = new Mock<IWorkItemsRepository>();
        var service = new WorkItemsService(repo.Object);

        var item = new WorkItem { Title = "Test" };
        var generatedId = Guid.NewGuid();

        repo.Setup(r => r.Add(item)).Returns(generatedId);

        var result = service.Add(item);

        Assert.Equal(generatedId, result);
        repo.Verify(r => r.Add(item), Times.Once);
        repo.Verify(r => r.SaveChanges(), Times.Once);
    }
    
    
    
    [Fact]
    public void MarkCompleted_ShouldSetIsCompleted_AndSave()
    {
        var repo = new Mock<IWorkItemsRepository>();
        var service = new WorkItemsService(repo.Object);

        var id = Guid.NewGuid();
        var item = new WorkItem { Id = id, IsCompleted = false };

        repo.Setup(r => r.Get(id)).Returns(item);

        var ok = service.MarkCompleted(id);

        Assert.True(ok);
        Assert.True(item.IsCompleted);

        repo.Verify(r => r.Update(item), Times.Once);
        repo.Verify(r => r.SaveChanges(), Times.Once);
    }

    
    [Fact]
    public void Remove_ShouldCallRepoRemove_AndSave()
    {
        var repo = new Mock<IWorkItemsRepository>();
        var service = new WorkItemsService(repo.Object);

        var id = Guid.NewGuid();

        repo.Setup(r => r.Remove(id)).Returns(true);

        var ok = service.Remove(id);

        Assert.True(ok);

        repo.Verify(r => r.Remove(id), Times.Once);
        repo.Verify(r => r.SaveChanges(), Times.Once);
    }
}