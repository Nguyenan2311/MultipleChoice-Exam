using Exemination.Domain.AggregateModels.SeedWork;
using MongoDB.Bson.Serialization.Attributes;

namespace Exemination.Domain.AggregateModels.CategoryAggregate;

public class Category:Entity
{   
    [BsonElement("name")]
     public string Name {get;set;}
     [BsonElement("urlPath")]
     public string UrlPath {get;set;}
     
        
}
