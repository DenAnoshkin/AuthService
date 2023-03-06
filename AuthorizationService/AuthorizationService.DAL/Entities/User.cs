
namespace AuthorizationService.DAL.Entities
{
    public class User : BaseEntity
    {
        
        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? Email { get; set; }
        
        public virtual UserGroup Group { get; set; } = null!;

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = null!;
    }
}
