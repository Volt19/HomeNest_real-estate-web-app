namespace HomeNest.Data.Models;

public class Property
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public int Price { get; set; }
    public int Area { get; set; }
    public int Rooms { get; set; }
    public string District { get; set; } = "";
    public string Type { get; set; } = "";
    public bool Furnished { get; set; }
    public int Floor { get; set; }
    public string Image { get; set; } = "";
    public string Features { get; set; } = ""; // comma-separated

    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public List<string> FeaturesList
    {
        get => string.IsNullOrEmpty(Features) ? new List<string>() : Features.Split(',').ToList();
        set => Features = value != null ? string.Join(",", value) : "";
    }

    public string PriceUnit { get; set; } = "";
    public int? OwnerId { get; set; }
    public User? Owner { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<Favorite> Favorites { get; set; } = new();
}
