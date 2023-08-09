namespace ReadIt.BLL.Models.BookModels
{
    public record BookUpdateModel : BookCreateModel
    {
        public Guid Id { get; set; }
    }
}