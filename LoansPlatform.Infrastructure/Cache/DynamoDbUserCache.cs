using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using LoansPlatform.Domain.Entities;
using LoansPlatform.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LoansPlatform.Infrastructure.Cache
{
    public class DynamoDbUserCache : IUserCache
    {
        private readonly IAmazonDynamoDB _dynamoDb;
        private const string TableName = "UsersCache";

        public DynamoDbUserCache(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }

        public async Task<User?> GetUserAsync(Guid id)
        {
            var request = new GetItemRequest
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S = id.ToString() } }
                }
            };

            var response = await _dynamoDb.GetItemAsync(request);
            if (response.Item == null || response.Item.Count == 0)
                return null;

            var json = response.Item["Data"].S;
            return JsonSerializer.Deserialize<User>(json);
        }

        public async Task SaveUserAsync(User user)
        {
            var request = new PutItemRequest
            {
                TableName = TableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S = user.Id.ToString() } },
                    { "Data", new AttributeValue { S = JsonSerializer.Serialize(user) } }
                }
            };

            await _dynamoDb.PutItemAsync(request);
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var request = new DeleteItemRequest
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S = id.ToString() } }
                }
            };

            await _dynamoDb.DeleteItemAsync(request);
        }

    }
}
