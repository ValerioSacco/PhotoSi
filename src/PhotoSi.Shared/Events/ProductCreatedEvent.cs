namespace PhotoSi.Shared.Events
{
    public record ProductCreatedEvent(
        Guid id, 
        string name, 
        string description, 
        decimal price, 
        string categoryName
    );
}
