using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Models.DTO;
using FormCraft.Application.Models.RequestModels;

namespace FormCraft.Application.Commands
{
    public record CreateNewFormWithQuestionCommand(
        NewTemplateRequest newTemplateRequest) : ICommand;
}
