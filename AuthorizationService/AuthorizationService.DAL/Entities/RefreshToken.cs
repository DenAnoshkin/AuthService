namespace AuthorizationService.DAL.Entities
{
    public class RefreshToken : BaseEntity
    {

        public string Token { get; set; } = null!;

        public User User { get; set; } = null!;
    }
}