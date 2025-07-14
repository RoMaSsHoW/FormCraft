using FormCraft.Application.Common.Messaging;

namespace FormCraft.Application.Commands.FormCommands
{
    public record SetAnswerToQuestionCommand(
        Guid QuestionId,
        string AnswerValue) : ICommand;
}
