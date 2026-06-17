using FluentValidation;
using Shared.DTOs;

namespace Core.Validators;

public class CreateJournalEntryValidator : AbstractValidator<CreateJournalEntryDto>
{
    public CreateJournalEntryValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

        RuleFor(x => x.Details)
            .NotEmpty().WithMessage("At least one journal entry detail is required")
            .Must(x => x.Count >= 2).WithMessage("Journal entry must have at least 2 details (debit and credit)");

        RuleForEach(x => x.Details)
            .SetValidator(new CreateJournalEntryDetailValidator());
    }
}

public class CreateJournalEntryDetailValidator : AbstractValidator<CreateJournalEntryDetailDto>
{
    public CreateJournalEntryDetailValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Account ID is required");

        RuleFor(x => x.Debit)
            .GreaterThanOrEqualTo(0).WithMessage("Debit amount cannot be negative");

        RuleFor(x => x.Credit)
            .GreaterThanOrEqualTo(0).WithMessage("Credit amount cannot be negative");

        RuleFor(x => x)
            .Must(x => x.Debit > 0 || x.Credit > 0)
            .WithMessage("Either debit or credit amount must be greater than 0");
    }
}
