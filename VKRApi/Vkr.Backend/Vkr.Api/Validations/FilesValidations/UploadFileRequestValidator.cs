using FluentValidation;
using Vkr.API.Contracts.FilesContracts;
using File = Vkr.Domain.Entities.Files.File;

namespace Vkr.API.Validations.FilesValidations
{
    public class UploadFileRequestValidator : AbstractValidator<FileUploadRequest>
    {
        public UploadFileRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Поле 'Имя файла' не может быть пустым")
                .MaximumLength(File.NameMaxLength).WithMessage($"Название файла не может быть длиннее {File.NameMaxLength} символов");

            RuleFor(x => x.Description)
                .MaximumLength(File.DescriptionMaxLength)
                .WithMessage($"Описание файла не может быть длиннее {File.DescriptionMaxLength} символов");
        }
    }
}
