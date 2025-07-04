using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Models.RequestModels;

namespace FormCraft.Application.Commands
{
    public record CreateNewFormCommand(
        //Guid AuthorId,
        string Title,
        string Description,
        //string ImageUrl,
        string Topic,
        IEnumerable<string> Tags,
        IEnumerable<QuestionRequest> Questions,
        bool IsPublic) : ICommand;
}


//брать юзера из контекста
//Получить пэйлоад клаймов прикрепить к контексту