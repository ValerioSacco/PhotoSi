namespace PhotoSi.Shared.Events
{
    public record UserCreatedEvent(
        Guid id, 
        string firstname, 
        string lastname, 
        string country, 
        string city, 
        string street, 
        string postalCode
    );
}
