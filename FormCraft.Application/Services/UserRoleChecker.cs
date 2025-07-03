using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormCraft.Application.Services
{
    public class UserRoleChecker : IUserRoleChecker
    {
        public bool IsAdmin(Guid userId)
        {
            return true; //временное решение
        }
    }
}
