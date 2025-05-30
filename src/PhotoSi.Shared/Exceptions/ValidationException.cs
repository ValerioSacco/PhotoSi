namespace PhotoSi.Shared.Exceptions
{
    public class ValidationException(IEnumerable<ValidationError> Errors)
        : Exception("Validation failed")
    {
        public IEnumerable<ValidationError> Errors { get; } = Errors;
    }

    public record ValidationError(string propertyName, string errorMessage);

}