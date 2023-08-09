using FluentValidation;
using ReadIt.BLL.Models.BookModels;

namespace ReadIt.WebApi.Validators.BookValidators
{
    public class BookUpdateValidator : AbstractValidator<BookUpdateModel>
    {
        public BookUpdateValidator()
        {
            RuleFor(u => u.Title).NotEmpty();
            RuleFor(u => u.Author).NotEmpty();
            RuleFor(u => u.TotalPages).NotEmpty();
            RuleFor(u => u.Id).NotEmpty();
        }
    }
}
