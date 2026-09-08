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
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.description)
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(10).WithMessage("Description must be at least 10 characters long.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        }
    }
}
