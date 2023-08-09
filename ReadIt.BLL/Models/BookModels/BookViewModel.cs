using ReadIt.DAL.Entities.Enums;

namespace ReadIt.BLL.Models.BookModels
{
    public record BookViewModel : BookUpdateModel
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public string Author { get; init; }
        public int TotalPages { get; init; }
        public Genre Genre { get; init; }
    }
}