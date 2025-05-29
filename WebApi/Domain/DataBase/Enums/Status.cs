using System.ComponentModel;

namespace Domain.DataBase.Enums;

public enum Status
{
    [Description("Не назначена")]
    NotAssigned,

    [Description("В работе")]
    AtWork,

    [Description("Решена")]
    Solved
}
