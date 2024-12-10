using EBC.Core.Models.Commons;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBC.Data.DTOs.User;

public class UserCreateEditDTO : BaseEntityCreateDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }

    public bool ImagePath { get; set; }

    [NotMapped]
    public IFormFile Image { get; set; }

    public Guid? UserTypeId { get; set; }
}
