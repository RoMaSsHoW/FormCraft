using FormCraft.Application.Common.Messaging;

namespace FormCraft.Application.Commands.FormWithAnswerCommands
{
    public record SetAnswerToQuestionCommand(
        Guid QuestionId,
        string AnswerValue) : ICommand;
}
