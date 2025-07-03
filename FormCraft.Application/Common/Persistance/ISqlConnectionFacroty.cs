using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormCraft.Application.Common.Persistance
{
    public interface ISqlConnectionFacroty
    {
        IDbConnection Create();
    }
}
