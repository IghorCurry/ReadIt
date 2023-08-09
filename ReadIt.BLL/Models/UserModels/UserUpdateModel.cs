namespace ReadIt.BLL.Models.UserModels;

public record UserUpdateModel : UserCreateModel
{
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}