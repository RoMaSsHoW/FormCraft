using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Models.DTO;
using FormCraft.Application.Models.RequestModels;

namespace FormCraft.Application.Commands
{
    public record CreateNewFormWithQuestionCommand(
        string Title,
        string Description,
        string Topic,
        IEnumerable<string> Tags,
        bool IsPublic,
        IEnumerable<QuestionDTO> Questions) : ICommand;
}
