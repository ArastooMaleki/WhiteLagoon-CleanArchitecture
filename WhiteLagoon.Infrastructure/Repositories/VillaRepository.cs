using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repositories
{
    public class VillaRepository(ApplicationDbContext dbContext) : Repository(dbContext), IVillaRepository
    {
        private readonly ApplicationDbContext dbContext = dbContext;
    }
}
