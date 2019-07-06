namespace Models.Users
{
    public class UserPatchInfo
    {
        public string Username { get; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public UserPatchInfo(string username, string oldPassword = null, string password = null,
            string confirmPassword = null)
        {
            Username = username;
            OldPassword = oldPassword;
            Password = password;
            ConfirmPassword = confirmPassword;
        }
    }
}