using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Models.ViewModels;

namespace FormCraft.Application.Commands
{
    public record UpdateTemplateCommand(TemplateView newTemplateInformation) : ICommand;
}
