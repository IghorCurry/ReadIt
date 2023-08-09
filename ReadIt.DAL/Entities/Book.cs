using ReadIt.DAL.Entities.Enums;

namespace ReadIt.DAL.Entities
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public int TotalPages { get; set; }
        public Genre Genre { get; set; }

    }
}
