using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using LoansPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LoansPlatform.Domain.Interfaces;

namespace LoansPlatform.Infrastructure.Cache
{
    public class DynamoDbLoanCache : ILoanCache
    {
        private readonly IAmazonDynamoDB _dynamoDb;
        private const string TableName = "LoansCache";

        public DynamoDbLoanCache(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }

        public async Task SaveLoanAsync(Loan loan)
        {
            var cleanLoan = new
            {
                loan.Id,
                loan.UserId,
                loan.Amount,
                loan.Status,
                loan.CreatedAt,
                loan.UpdatedAt
            };

            var item = new Dictionary<string, AttributeValue>
            {
                ["Id"] = new AttributeValue { S = loan.Id.ToString() },
                ["Data"] = new AttributeValue { S = JsonSerializer.Serialize(cleanLoan) }
            };

            await _dynamoDb.PutItemAsync(new PutItemRequest
            {
                TableName = TableName,
                Item = item
            });
        }


        public async Task<Loan?> GetLoanAsync(Guid id)
        {
            var response = await _dynamoDb.GetItemAsync(new GetItemRequest
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    ["Id"] = new AttributeValue { S = id.ToString() }
                }
            });

            if (response.Item.Count == 0)
                return null;

            return JsonSerializer.Deserialize<Loan>(response.Item["Data"].S);
        }

        public async Task DeleteLoanAsync(Guid id)
        {
            await _dynamoDb.DeleteItemAsync(new DeleteItemRequest
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    ["Id"] = new AttributeValue { S = id.ToString() }
                }
            });
        }



    }
}
