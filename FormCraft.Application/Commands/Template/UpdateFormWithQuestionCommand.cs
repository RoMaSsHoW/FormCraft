using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Models.ViewModels;

namespace FormCraft.Application.Commands.Template
{
    public record UpdateFormWithQuestionCommand(TemplateView NewTemplateInformation) : ICommand;
}
