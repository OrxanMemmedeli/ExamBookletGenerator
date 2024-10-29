using EBC.Core.Constants;
using System.ComponentModel;

namespace EBC.Core.Models.Enums;

public enum PaymentType
{
    [Description(TranslateEnToAz.Payment)]
    Payment = 0,
    [Description(TranslateEnToAz.Debt)]
    Debt = 1
}
