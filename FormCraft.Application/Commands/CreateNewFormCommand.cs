using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Models.DTO;

namespace FormCraft.Application.Commands
{
    public record CreateNewFormCommand(
        string Title,
        string Description,
        string Topic,
        IEnumerable<string> Tags,
        IEnumerable<QuestionDTO> Questions,
        bool IsPublic) : ICommand;
}


//брать юзера из контекста
//Получить пэйлоад клаймов прикрепить к контексту