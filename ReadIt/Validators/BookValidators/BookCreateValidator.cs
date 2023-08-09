using FluentValidation;
using ReadIt.BLL.Models.BookModels;

namespace ReadIt.WebApi.Validators.BookValidators
{
    public class BookCreateValidator : AbstractValidator<BookCreateModel>
    {
        public BookCreateValidator()
        {
            RuleFor(u => u.Title).NotEmpty();
            RuleFor(u => u.Author).NotEmpty();
            RuleFor(u => u.TotalPages).NotEmpty();
        }
    }
}
