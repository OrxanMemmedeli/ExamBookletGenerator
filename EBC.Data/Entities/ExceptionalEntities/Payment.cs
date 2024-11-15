using EBC.Core.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBC.Data.Entities.ExceptionalEntities;

public class Payment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public DateTime CreatedDate { get; set; }
    public decimal Amout { get; set; }
    public PaymentType PaymentType { get; set; }


    #region Referances 

    public Guid CompanyId { get; set; }
    public virtual Company Company { get; set; }

    #endregion
}
