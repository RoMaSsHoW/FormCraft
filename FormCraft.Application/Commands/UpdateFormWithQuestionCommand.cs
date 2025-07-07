using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Models.ViewModels;

namespace FormCraft.Application.Commands
{
    public record UpdateFormWithQuestionCommand(TemplateView NewTemplateInformation) : ICommand;
}
