using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.PaymentOrDebt;

public class PaymentCreateDTO : BaseEntityCreateDTO
{
    public decimal Amout { get; set; }
}
