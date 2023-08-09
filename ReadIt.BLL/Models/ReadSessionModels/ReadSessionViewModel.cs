namespace ReadIt.BLL.Models.ReadSessionModels
{
    public record ReadSessionViewModel : ReadSessionUpdateModel
    {
        public DateTime StartReadingDate { get; init; }
        public int StopPage { get; init; }
        public Guid UserId { get; init; }
        public Guid BookId { get; init; }
    }
}