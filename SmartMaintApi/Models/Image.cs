public class ImageFile
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public DateTimeOffset? Date { get; set; }
    public string? MimeType { get; set; }
    public string? FormattedDate => Date?.ToString("HH:mm dd.MM.yyyy");
    public string? FullImageUrl { get; set; } 
}
