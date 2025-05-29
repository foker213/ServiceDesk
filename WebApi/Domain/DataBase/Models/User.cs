using Microsoft.AspNetCore.Identity;

namespace Domain.DataBase.Models;

public class User : IdentityUser<int>, IEntity
{
    /// <summary>
    /// ФИО
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Выполняемая заявка
    /// </summary>
    public Request? Request { get; set; }

    /// <summary>
    /// Заблокирован
    /// </summary>
    public bool Blocked { get; set; }

    /// <summary>
    /// Время блокировки
    /// </summary>
    public DateTime? BlockedAt { get; set; }

    /// <summary>
    /// Причина блокировки
    /// </summary>
    public string? BlockedReason { get; set; }

    /// <summary>
    /// Время последнего входа
    /// </summary>
    public DateTime? LastLogonTime { get; set; }

    /// <summary>
    /// Время создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Время обновления
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

}
