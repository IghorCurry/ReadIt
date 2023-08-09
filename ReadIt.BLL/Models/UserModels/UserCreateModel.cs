namespace ReadIt.BLL.Models.UserModels
{
    public record UserCreateModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}