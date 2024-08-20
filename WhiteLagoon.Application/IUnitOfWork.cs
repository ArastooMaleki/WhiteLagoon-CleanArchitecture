using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Application
{
    /// <summary>
    /// aggregate all the repositry to gether 
    /// </summary>
    public interface IUnitOfWork
    {
        IVillaRepository VillaRepository { get;}
        /// <summary>
        /// use this instead of dbcontext.SaveChangesAsync()
        /// </summary>
        /// <returns></returns>
        Task SaveAsync();
    }
}
