namespace JustLabel.Models;

public class ImageModel
{
    public int Id { get; set; }

    public int DatasetId { get; set; }

    public string Path { get; set; }
    
    public int Width { get; set; }

    public int Height { get; set; }
}
