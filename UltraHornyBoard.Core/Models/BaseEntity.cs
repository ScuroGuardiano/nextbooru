using System.ComponentModel.DataAnnotations.Schema;

namespace UltraHornyBoard.Models;

public class BaseEntity
{
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}