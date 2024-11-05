using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Optix.DataAccess.DbModels;

public class EntityObject
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
}