using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Models.RequestModels;

namespace FormCraft.Application.Commands.FormWithAnswerCommands
{
    public record DeleteAnswersFromQoestionsCommand(
        IEnumerable<AnswersToDeleteRequestModel> AnswersToDelete) : ICommand;
}
