using dnd_assistant.Abstract;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dnd_assistant.Models
{
    [Table("RefreshTokens")]
    [Index(nameof(TokenHash), IsUnique = true)]
    public class RefreshToken : GuidEntity
    {
        [Required]
        public Guid UserID { get; set; }

        [ForeignKey(nameof(UserID))]
        public User User { get; set; } = null!;

        [Required]
        [MaxLength(64)]
        public string TokenHash { get; set; } = null!;

        [Required]
        [Column(TypeName = "timestamp with time zone")]
        public DateTime Expires { get; set; }

        [Required]
        [Column(TypeName = "timestamp with time zone")]
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string? CreatedByIp { get; set; }

        [Column(TypeName = "timestamp with time zone")]
        public DateTime? Revoked { get; set; }

        [MaxLength(100)]
        public string? RevokedByIp { get; set; }

        [MaxLength(64)]
        public string? ReplacedByTokenHash { get; set; }

        [NotMapped]
        public bool IsExpired => DateTime.UtcNow >= Expires;

        [NotMapped]
        public bool IsActive => Revoked == null && !IsExpired;
    }
}
