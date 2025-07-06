using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormCraft.Domain.Aggregates.UserAggregate.Interfaces
{
    public interface ICurrentUserService
    {
        Guid? GetUserId();
        string? GetRole();
        bool IsAuthenticated();
    }
}
