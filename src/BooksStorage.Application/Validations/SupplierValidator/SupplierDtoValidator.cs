namespace BooksStorage.Application.Validations.SupplierValidator
{
    public class SupplierDtoValidator : AbstractValidator<SupplierDto>
    {
        public SupplierDtoValidator()
        {
            RuleFor(x => x.SupplierName)
                .NotEmpty()
                .WithMessage("Имя обязательно.")
                .MaximumLength(100)
                .WithMessage("Имя не должно превышать 100 знаков.");
            
        }
    }
}
