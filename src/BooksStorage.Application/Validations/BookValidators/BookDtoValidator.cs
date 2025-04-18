namespace BooksStorage.Application.Validations.BookValidators
{
    public class BookDtoValidator : AbstractValidator<BookDto>
    {
        public BookDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Нужно имя автора.")
                .MaximumLength(100)
                .WithMessage("Титул должен быть не болле 100 букв.");
            RuleFor(x => x.Author)
                .NotEmpty()
                .WithMessage("Нужно имя автора.")
                .MaximumLength(50)
                .WithMessage("Имя автора не менее 50 букв.");
            RuleFor(x => x.Price)
                .NotEmpty()
                .WithMessage("Нужна цена.")
                .GreaterThan(0)
                .WithMessage("Цена должна быть больше 0.");

        }
    }
}
