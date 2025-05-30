namespace PhotoSi.UsersService.Models
{
    public abstract class BaseModel
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int Version { get; set; } = 1;

    }
}
