namespace newProject.Domain.Common;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
} 