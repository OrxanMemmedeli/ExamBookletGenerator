namespace EBC.Core.Models.Dtos.Identities.User;

public class UserLoginResponseDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }

    public string Roles { get; set; }
    public string Organizations { get; set; }
}
