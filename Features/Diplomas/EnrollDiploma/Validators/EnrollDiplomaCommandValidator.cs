using exam_system.Features.Diplomas.EnrollDiploma.Commands;
using FluentValidation;

namespace exam_system.Features.Diplomas.EnrollDiploma.Validators
{
    public class EnrollDiplomaCommandValidator : AbstractValidator<EnrollDiplomaCommand>
    {
        public EnrollDiplomaCommandValidator()
        {
            RuleFor(x => x.DiplomaId).NotEmpty();
            RuleFor(x => x.StudentId).NotEmpty();
        }
    }
}
