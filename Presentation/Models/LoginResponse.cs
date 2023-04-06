namespace Presentation.Models
{
    public class LoginResponse
    {
        public LoginResponse()
        {
            ErrorsMessage = new List<string>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public bool Sucess { get; set; }
        public List<string> ErrorsMessage { get; private set; }
    }
}
