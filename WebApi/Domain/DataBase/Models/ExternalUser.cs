namespace ServiceDesk.Domain.Database.Models;

public class ExternalUser : Entity
{
    /// <summary>
    /// ФИО
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Номер телефона
    /// </summary>
    public string? NumberPhone { get; set; }

    /// <summary>
    /// Почтовый адрес
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Прикрепленный чат
    /// </summary>
    public Chat? Chat { get; set; }
}
