using FluentValidation;
using ReadIt.BLL.Models.ReadSessionModels;

namespace ReadIt.WebApi.Validators.ReadSessionValidators
{
    public class ReadSessionUpdateValidator : AbstractValidator<ReadSessionUpdateModel>
    {
        public ReadSessionUpdateValidator()
        {
            RuleFor(u => u.BookId).NotEmpty();
            RuleFor(u => u.UserId).NotEmpty();
            RuleFor(u => u.StopPage).NotEmpty();
            RuleFor(u => u.Id).NotEmpty(); ;
        }
    }
}
