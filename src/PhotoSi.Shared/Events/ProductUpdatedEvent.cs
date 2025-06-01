namespace PhotoSi.Shared.Events
{
    public record ProductUpdatedEvent(
        Guid id,
        string name,
        string description,
        decimal price,
        string categoryName
    );
}
