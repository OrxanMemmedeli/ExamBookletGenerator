using EBC.Core.Constants;
using System.ComponentModel;

namespace EBC.Core.Models.Enums;

/// <summary>
/// E-poçt şablon növlərini ifadə edən enum.
/// </summary>
public enum EmailTemplateType
{
    [Description(TranslateEnToAz.Debt)]
    Debt = 1,

    [Description(TranslateEnToAz.Payment)]
    Payment = 2,

    [Description(TranslateEnToAz.Welcome)]
    Welcome = 3,

    [Description(TranslateEnToAz.ComeBack)]
    ComeBack = 4,

    [Description(TranslateEnToAz.Blocked)]
    Blocked = 5,

    [Description(TranslateEnToAz.StopSubscription)]
    StopSubscription = 6,

    [Description(TranslateEnToAz.Confirm)]
    Confirm = 7
}
