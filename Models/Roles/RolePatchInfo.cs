namespace Models.Roles
{
    public class RolePatchInfo
    {
        public string UserId { get; }
        public string UserRole { get; set; }

        public RolePatchInfo(string userId, string userRole = null)
        {
            UserId = userId;
            UserRole = userRole;
        }
    }
}