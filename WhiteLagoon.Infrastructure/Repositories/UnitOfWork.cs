using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationDbContext _dbContext { set; get; }
        public IVillaRepository VillaRepository {  get; private set; }

        public IVillaNumberRepository VillaNumberRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            VillaRepository = new VillaRepository(dbContext);
            VillaNumberRepository = new VillaNumberRepository(dbContext);
        }
        public async Task SaveAsync()
        {
            _dbContext.SaveChanges();
        }
    }
}
