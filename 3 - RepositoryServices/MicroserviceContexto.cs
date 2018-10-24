using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Microservice.Repository
{
    public class MicroserviceContexto
    {
        private IConfiguration _configuration;
        public readonly IMongoDatabase _db;
        protected readonly IMongoClient _client;

        public MicroserviceContexto(IConfiguration config)
        {
            _configuration = config;
            _client = new MongoClient(_configuration.GetConnectionString("ConexaoMongoDB"));
            _db = _client.GetDatabase(_configuration["DatabaseMongoDB"]);
        }        
    }
}