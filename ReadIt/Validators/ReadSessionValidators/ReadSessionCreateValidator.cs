using FluentValidation;
using ReadIt.BLL.Models.ReadSessionModels;

namespace ReadIt.WebApi.Validators.ReadSessionValidators
{
    public class ReadSessionCreateValidator : AbstractValidator<ReadSessionCreateModel>
    {
        public ReadSessionCreateValidator()
        {
            RuleFor(u => u.BookId).NotEmpty();
            RuleFor(u => u.UserId).NotEmpty();
            RuleFor(u => u.StopPage).NotEmpty();
        }
    }
}
