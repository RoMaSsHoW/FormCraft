using FormCraft.Application.Common.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormCraft.Application.Commands
{
    public class CreateNewFormWithQuestionCommandHandler : ICommandHandler<CreateNewFormWithQuestionCommand>
    {
        public Task Handle(CreateNewFormWithQuestionCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
