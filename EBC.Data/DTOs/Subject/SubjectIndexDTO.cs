using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.Subject;

public class SubjectIndexDTO : BaseEntityViewDTO
{
    public string Name { get; set; }
    public decimal? AmountForQuestion { get; set; }
}
