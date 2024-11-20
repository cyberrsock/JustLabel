namespace JustLabel.Models;

public class UserModel
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string Salt { get; set; }

    public string RefreshToken { get; set; }

    public bool IsAdmin { get; set; }

    public bool BlockMarks { get; set; }
}
