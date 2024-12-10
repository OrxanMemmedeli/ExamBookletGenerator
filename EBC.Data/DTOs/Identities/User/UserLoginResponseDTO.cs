namespace EBC.Data.DTOs.Identities.User;

public class UserLoginResponseDTO
{
    public required Guid UserId { get; set; }
    public required bool IsAdmin { get; set; }
    public required bool IsManager { get; set; }
    public required string UserName { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string FullName { get; set; }

    public required string Roles { get; set; }
    public required string Organizations { get; set; }
    public required string CompanyIds { get; set; }

    public required DateTime LoginTime { get; set; }
}
