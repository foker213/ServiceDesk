using Microsoft.AspNetCore.Identity;

namespace ServiceDesk.Domain.Database.Models;

public class UserRole : IdentityRole<int>, IEntity
{
    /// <summary>
    /// Время создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Время обновления
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
