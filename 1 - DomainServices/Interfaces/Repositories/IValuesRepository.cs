using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservice.DomainServices.Entities;

namespace Microservice.DomainServices.Interfaces.Repositories
{
    public interface IValuesRepository
    {
         Task AddRangeAsync(List<Values> values);
         Task<List<Values>> FindAsync(string tenantID);
    }
}