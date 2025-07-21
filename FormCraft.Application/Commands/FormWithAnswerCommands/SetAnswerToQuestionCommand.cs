using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Models.RequestModels;

namespace FormCraft.Application.Commands.FormWithAnswerCommands
{
    public record SetAnswerToQuestionCommand(
        SetQuestionAnswerRequestModel SetQuestionAnswer) : ICommand;
}
