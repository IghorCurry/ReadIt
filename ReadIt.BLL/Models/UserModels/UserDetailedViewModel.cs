namespace ReadIt.BLL.Models.UserModels
{
    public record UserDetailedViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public IList<string> RoleNames { get; set; }
    }
}
