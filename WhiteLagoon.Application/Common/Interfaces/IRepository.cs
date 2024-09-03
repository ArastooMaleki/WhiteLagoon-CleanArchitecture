using System;
using System.Linq.Expressions;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Interfaces
{
    /// <summary>
    /// villa repo
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// returns all the existing villas 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Villa>> GetAllAsync(Expression<Func<Villa, bool>>? filter = null, bool tracked = false);
        /// <summary>
        /// return a speciall villa witch match the filter 
        /// </summary>
        /// <param name="filter">a lambda expresion represent the proper filter</param>
        /// <returns>a villa according to fillter</returns>
        Task<Villa?>? GetAsync(Expression<Func<Villa, bool>>? filter = null, bool tracked = false);
        /// <summary>
        /// will delete the data
        /// </summary>
        /// <param name="villa">a villa to be deleted</param>
        /// <returns>nothing</returns>
        Task DeleteAsync(Villa villa);
        /// <summary>
        /// update the villa and tracke it 
        /// </summary>
        /// <param name="villa">a villa object to update</param>
        /// <returns></returns>
        Task UpdateAsync(Villa villa);
        /// <summary>
        /// Add the new villa to entity 
        /// </summary>
        /// <param name="villa">a new villa object</param>
        /// <returns></returns>
        Task AddAsync(Villa villa);

    }
}
