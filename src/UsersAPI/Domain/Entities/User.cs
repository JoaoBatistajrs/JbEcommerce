using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UsersAPI.Domain.Enum;

namespace UsersAPI.Domain.Entities;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(250)]
    public string Name { get; set; } = default!;

    [StringLength(50)]
    public string Email { get; set; } = default!;

    [StringLength(255)]
    public string Password { get; set; } = default!;

    [Required]
    public UserRole Role { get; set; } = UserRole.User;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
