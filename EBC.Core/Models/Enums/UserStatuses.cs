using EBC.Core.Constants;
using System.ComponentModel;

namespace EBC.Core.Models.Enums;

public enum UserStatuses
{
    [Description(TranslateEnToAz.Active)]
    Active = 0,

    [Description(TranslateEnToAz.Deaktive)]
    Deaktive = 1
}
