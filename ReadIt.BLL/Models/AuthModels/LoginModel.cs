namespace ReadIt.BLL.Models.AuthModels
{
    public record LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
