namespace CarXRMWebAPI.Models
{
    public class LoginUser : ILoginUser
    {
        private string _userName = string.Empty;
        private string _password = string.Empty;

        public string UserName { get => _userName; set => _userName = value; }
        public string Password { get => _password; set => _password = value; }
    }
}