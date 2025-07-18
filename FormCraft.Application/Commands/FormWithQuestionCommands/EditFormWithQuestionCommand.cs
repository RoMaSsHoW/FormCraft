using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Models.RequestModels;

namespace FormCraft.Application.Commands.FormWithQuestionCommands
{
    public record EditFormWithQuestionCommand(
        Guid FormId,
        string? Title,
        string? Description,
        string? TopicName,
        bool IsPublic,
        long LastVersion,
        IEnumerable<string> Tags,
        IEnumerable<QuestionRequest> Questions) : ICommand;
}
