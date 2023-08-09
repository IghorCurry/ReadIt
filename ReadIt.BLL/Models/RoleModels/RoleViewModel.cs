namespace ReadIt.BLL.Models.RoleModels
{
    public record RoleViewModel : RoleCreateModel
    {
        public Guid Id { get; set; }
    }
}
