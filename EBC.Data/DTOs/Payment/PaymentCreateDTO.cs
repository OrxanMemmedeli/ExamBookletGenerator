using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.Payment;

public class PaymentCreateDTO : BaseEntityCreateDTO
{
    public decimal Amout { get; set; }
}
