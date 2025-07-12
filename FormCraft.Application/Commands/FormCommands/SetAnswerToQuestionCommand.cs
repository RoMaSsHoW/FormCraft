using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormCraft.Application.Commands.FormCommands
{
    public record SetAnswerToQuestionCommand(AnswerRequest AnswerRequest) : ICommand;
}
