using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Models.RequestModels;

namespace FormCraft.Application.Commands.FormCommands
{
    public record DeleteAnswersFromQoestionsCommand(
        IEnumerable<AnswersToDeleteRequestModel> AnswersToDeleteRequest) : ICommand;
}
