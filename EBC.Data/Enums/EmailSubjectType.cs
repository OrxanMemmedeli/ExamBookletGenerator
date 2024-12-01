using System.ComponentModel;

namespace EBC.Data.Enums;

public enum EmailSubjectType
{
    [Description("Borc")]
    Debt = 1,
    [Description("Ödəniş")]
    Payment = 2,
    [Description("Xoş gəldin")]
    Welcome = 3,
    [Description("Abonəliyin bərpası")]
    ComeBack = 4,
    [Description("Əngəlləndi")]
    Blocked = 5,
    [Description("Abonəlik donduruldu")]
    StopSubscription = 6,
    [Description("E-maili təsdiqlə")]
    Confirm = 7,
}
