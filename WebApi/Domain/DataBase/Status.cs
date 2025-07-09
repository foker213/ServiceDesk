using System.ComponentModel;

namespace ServiceDesk.Domain.Database;

public enum Status
{
    [Description("Не назначена")]
    NotAssigned,

    [Description("В работе")]
    AtWork,

    [Description("Решена")]
    Solved
}
