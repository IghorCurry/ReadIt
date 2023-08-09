namespace ReadIt.BLL.Models.ReadSessionModels;

public record ReadSessionUpdateModel : ReadSessionCreateModel
{
    public Guid Id { get; init; }
}