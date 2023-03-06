namespace AuthorizationService.DAL.Entities
{
    public class Permission : BaseEntity
    {
        public string Action { get; set; } = null!;

        public string? Name { get; set; }

        public virtual ICollection<UserGroup>? UserGroups { get; set; }
    }
}