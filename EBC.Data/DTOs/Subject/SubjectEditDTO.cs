using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.Subject;

public class SubjectEditDTO : BaseEntityEditDTO
{
    public string Name { get; set; }
    public decimal? AmountForQuestion { get; set; }
}
