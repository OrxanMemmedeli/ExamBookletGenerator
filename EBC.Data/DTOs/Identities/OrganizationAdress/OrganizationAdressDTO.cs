using EBC.Core.CustomFilter.WorkFilter.Filters;
using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.Identities.OrganizationAdress;

public class OrganizationAdressDTO : BaseEntityViewDTO
{
    public string RequestAdress { get; set; }

    public FilterOperation RequestAdressFilterType { get; init; } = FilterOperation.Contains;

}