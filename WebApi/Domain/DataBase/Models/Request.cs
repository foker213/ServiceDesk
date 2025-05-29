using Domain.DataBase.Enums;

namespace Domain.DataBase.Models;

public class Request : Entity
{
    /// <summary>
    /// Прикрепленный пользователь
    /// </summary>
    public User? User { get; set; } = default!;
    public int UserId { get; set; }

    /// <summary>
    /// Статус заявки
    /// </summary>
    public Status Status { get; set; }

    /// <summary>
    /// Чат
    /// </summary>
    public Chat? Chat { get; set; } = default!;
    public int ChatId { get; set; }

    /// <summary>
    /// Дата начала выполнения заявки
    /// </summary>
    public DateTime? DateStartRequest { get; set; }

    /// <summary>
    /// Дата завершения выполнения заявки
    /// </summary>
    public DateTime? DateEndRequest { get; set; }
}
