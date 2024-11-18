namespace EBC.Core.Models.Dtos.Identities.User;

public class UserLoginResponseDTO
{
    public Guid UserId { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsManager { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }

    public string Roles { get; set; }
    public string Organizations { get; set; }

    public DateTime LoginTime { get; set; }
    public Guid CompanyId { get; set; }
}
