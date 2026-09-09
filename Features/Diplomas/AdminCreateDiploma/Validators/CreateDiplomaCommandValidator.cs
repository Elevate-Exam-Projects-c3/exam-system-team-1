using exam_system.Features.Diplomas.AdminCreateDiploma.Commands;
using FluentValidation;

namespace exam_system.Features.Diplomas.AdminCreateDiploma.Validators
{
    public class CreateDiplomaCommandValidator:AbstractValidator<CreateDiplomaCommand>
    {
        public CreateDiplomaCommandValidator()
        {
            RuleFor(x => x.title)
                .NotEmpty().WithMessage("Title is required.")
                .MinimumLength(3).WithMessage("Title must be at least 3 characters long.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

            RuleFor(x => x.description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");
        }
    }
}
