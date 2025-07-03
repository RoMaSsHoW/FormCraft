using FormCraft.Application.Common.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormCraft.Application.Queries
{
    public record GetAllFormsQuery(Guid id) : IQuery;
}
