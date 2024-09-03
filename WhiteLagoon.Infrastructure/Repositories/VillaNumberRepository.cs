using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repositories
{
    public class VillaNumberRepository : Repository , IVillaNumberRepository
    {
        public VillaNumberRepository(ApplicationDbContext dbContext):base(dbContext)
        {
            
        }
    }
}
