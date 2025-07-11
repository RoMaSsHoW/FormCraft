using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Models.DTO;

namespace FormCraft.Application.Commands
{
    public record CreateNewFormWithQuestionCommand(
        string Title,
        string Description,
        string Topic,
        IEnumerable<string> Tags,
        IEnumerable<QuestionDTO> Questions,
        bool IsPublic = true) : ICommand;
}
