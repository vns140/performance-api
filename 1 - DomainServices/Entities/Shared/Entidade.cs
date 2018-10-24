using System;
using MongoDB.Bson;

namespace Microservice.DomainServices.Entities.Shared
{
    public class Entity
    {   public ObjectId _id { get; set; }
        public string TenantID { get; set; }
    }
}