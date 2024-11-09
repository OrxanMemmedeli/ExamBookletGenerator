namespace EBC.Core.Helpers.Authentication;

public class CustomUser
{
    public Guid Id { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsManager { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}"; // Avtomatik olaraq tam adı yaradır
    public string Roles { get; set; }
    public string OrganizationAddress { get; set; }
}