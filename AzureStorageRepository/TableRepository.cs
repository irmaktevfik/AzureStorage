using AzureStorageRepository.Helper;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageRepository
{
    public class TableRepository : AzureStorageRepository
    {
        CloudTableClient tableClient;
        CloudTable table;

        public TableRepository()
        {
            tableClient = base.storageAccount.CreateCloudTableClient();
        }

        /// <summary>
        /// Creates the table with the given tablename if it does not exists
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<bool> CreateTableAsync(string tableName)
        {
            // Create the table if it doesn't exist.
            table = tableClient.GetTableReference(tableName);
            return await table.CreateIfNotExistsAsync();
        }

        /// <summary>
        /// Deletes the given table if exists
        /// </summary>
        /// <returns></returns>
        public async Task DeleteTableAsync(string tableName)
        {
            // Delete the table 
            table = tableClient.GetTableReference(tableName);
            await table.DeleteIfExistsAsync();
        }

        /// <summary>
        /// inserts the given T to the given tablename
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableModel"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<bool> InsertAsync<T>(T tableModel, string tableName)
        {
            // Create the table if it doesn't exist.
            table = tableClient.GetTableReference(tableName);
            TableOperation insertOperation = TableOperation.Insert(tableModel as ITableEntity);
            var result = await table.ExecuteAsync(insertOperation);
            return HttpStatusCodeHelper.GetResultFromTableResult(result);
        }

        /// <summary>
        /// Inserts batch records of type T to the given table name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableModel"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<bool> InsertBatchAsync<T>(List<T> tableModel, string tableName)
        {
            //declare TableBatchOperation
            TableBatchOperation tableBatchOperation = new TableBatchOperation();
            // Create the table if it doesn't exist.
            table = tableClient.GetTableReference(tableName);

            foreach (var item in tableModel)
            {
                tableBatchOperation.Insert(tableModel as ITableEntity);
            }
            var result = await table.ExecuteBatchAsync(tableBatchOperation);
            return HttpStatusCodeHelper.GetResultFromTableListResult(result);
        }

        /// <summary>
        /// Gets all records without a parameter filtering
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllAsync<T>(string tableName)
             where T : ITableEntity, new()
        {
            Task.Factory.StartNew(async () =>
            {
                table = tableClient.GetTableReference(tableName);
                TableQuery<T> query = new TableQuery<T>();

                //Execute query async ?
                var result = table.ExecuteQuery(query);
                if (result != null)
                    return result as IEnumerable<T>;
                else
                    return null;
            }).Unwrap();
            return default(IEnumerable<T>);
        }

        /// <summary>
        /// Gets all records matching to the filter criteria
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="filter"></param>
        /// <returns>TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "something")</returns>
        public async Task<IEnumerable<T>> GetAllAsync<T>(string tableName, string filter)
            where T : ITableEntity, new()
        {
            table = tableClient.GetTableReference(tableName);
            TableQuery<T> query = new TableQuery<T>().Where(filter);
            var result = table.ExecuteQuery(query);
            if (result != null)
                return result as IEnumerable<T>;
            else
                return null;
        }

        /// <summary>
        /// Gets the single object with the given criteria
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="partitionKey"></param>
        /// <param name="rowKey"></param>
        /// <returns></returns>
        public async Task<T> GetSingleAsync<T>(string tableName, string partitionKey, string rowKey)
            where T : ITableEntity, new()
        {
            table = tableClient.GetTableReference(tableName);
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            TableResult retrievedResult = await table.ExecuteAsync(retrieveOperation);
            if (retrievedResult.Result != null)
                return ((T)retrievedResult.Result);
            else
                return default(T);
        }

        /// <summary>
        /// Updates an entity with the given credentials on update data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="partitionKey"></param>
        /// <param name="rowKey"></param>
        /// <param name="updateData"></param>
        /// <returns></returns>
        public async Task<bool> ReplaceSingleAsync<T>(string tableName, string partitionKey, string rowKey, T updateData,List<string> propertiesToUpdate)
            where T : ITableEntity, new()
        {
            table = tableClient.GetTableReference(tableName);
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            // Execute the retrieve operation.
            TableResult retrievedResult = await table.ExecuteAsync(retrieveOperation);

            T resultObject = (T)retrievedResult.Result;

            //Update
            if (resultObject != null)
            {
                resultObject = ReflectionPropertyMatcher.SetPropertyValues<T>(updateData, resultObject, propertiesToUpdate);
                // Create the InsertOrReplace TableOperation
                TableOperation updateOperation = TableOperation.Replace(resultObject);
                // Execute the operation.
                var returnData = await table.ExecuteAsync(updateOperation);
                return HttpStatusCodeHelper.GetResultFromTableResult(returnData);
            }
            else
                return false;

        }

        /// <summary>
        /// carries out InsertOrReplace for the given object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="partitionKey"></param>
        /// <param name="rowKey"></param>
        /// <param name="updateData"></param>
        /// <param name="updateData"></param>
        /// <returns></returns>
        public async Task<bool> InsertOrReplaceSingleAsync<T>(string tableName, string partitionKey, string rowKey, T updateData, List<string> propertiesToUpdate)
            where T : ITableEntity, new()
        {
            // Create the CloudTable object that represents the "tableName" table.
            table = tableClient.GetTableReference(tableName);

            // Create a retrieve operation that takes a "tableName" entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);

            // Execute the retrieve operation.
            TableResult retrievedResult = await table.ExecuteAsync(retrieveOperation);

            T resultObject = (T)retrievedResult.Result;

            //Update
            if (resultObject != null)
            {
                resultObject = ReflectionPropertyMatcher.SetPropertyValues<T>(updateData, resultObject, propertiesToUpdate);
                // Create the InsertOrReplace TableOperation
                TableOperation updateOperation = TableOperation.InsertOrReplace(resultObject);
                // Execute the operation.
                var returnData = await table.ExecuteAsync(updateOperation);
                return HttpStatusCodeHelper.GetResultFromTableResult(returnData);
            }
            else
                return false;

        }

        /// <summary>
        /// Delete the record for the given parameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="partitionKey"></param>
        /// <param name="rowKey"></param>
        /// <returns></returns>
        public async Task<bool> DeleteSingleAsync<T>(string tableName, string partitionKey, string rowKey)
            where T : ITableEntity, new()
        {
            // Create the CloudTable object that represents the "tableName" table.
            table = tableClient.GetTableReference(tableName);

            // Create a retrieve operation that takes a "tableName" entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);

            // Execute the retrieve operation.
            TableResult retrievedResult = await table.ExecuteAsync(retrieveOperation);

            if (retrievedResult.Result != null)
            {
                var data = ((T)retrievedResult.Result);
                TableOperation dOperation = TableOperation.Delete(data);
                var result = await table.ExecuteAsync(dOperation);
                return HttpStatusCodeHelper.GetResultFromTableResult(result);
            }
            else
                return false;
        }
    }
}
