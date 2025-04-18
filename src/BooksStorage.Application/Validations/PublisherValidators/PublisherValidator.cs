namespace BooksStorage.Application.Validations.PublisherValidators
{
    public class PublisherValidator : AbstractValidator<PublisherDto>
    {
        public PublisherValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Поле обязательно.")
                .MaximumLength(100)
                .WithMessage("Кол-во знаков не должно превышать 100.");

            RuleFor(x => x.ContactInfo)
                .MaximumLength(200)
                .WithMessage("Контактная инфо не должна превышать 200 знаков.");
        }
    }
}
