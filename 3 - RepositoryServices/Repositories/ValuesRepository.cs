using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservice.DomainServices.Entities;
using Microservice.DomainServices.Interfaces.Repositories;
using Microservice.Repository;
using MongoDB.Driver;

namespace Microservice.RepositoryServices.Repositories
{
    public class ValuesRepository : IValuesRepository
    {
        private readonly MicroserviceContexto _microserviceContexto;
        
        protected ValuesRepository(){}
        public ValuesRepository(MicroserviceContexto microserviceContexto)
        {
            _microserviceContexto = microserviceContexto;
        }

        public async Task AddRangeAsync(List<Values> values)
        {
            try
            {
                await _microserviceContexto._db.GetCollection<Values>("Values").InsertManyAsync(values);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task<List<Values>> FindAsync(string tenantID)
        {
            try
            {
                var filter = Builders<Values>.Filter.Eq("TenantID", tenantID);

                return await _microserviceContexto._db.GetCollection<Values>("Values").Find(filter).ToListAsync();
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}