
namespace AuthorizationService.DAL.Entities
{
    public class UserGroup : BaseEntity
    {

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public virtual ICollection<User>? Users { get; set; }

        public virtual ICollection<Permission>? Permissions { get; set; }

    }
}
