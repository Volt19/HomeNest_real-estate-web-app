namespace HomeNest.Data.Models;

public class UserSessionDto
{
    public int UserId { get; set; }
    public string UserName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public bool IsAdmin { get; set; }
}
