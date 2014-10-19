using AzureStorageRepository.Helper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;

namespace AzureStorageRepository
{
    public class QueuesRepository : AzureStorageRepository
    {
        CloudQueueClient queueClient;
        CloudQueue queue;

        public QueuesRepository()
        {
            queueClient = base.storageAccount.CreateCloudQueueClient();
        }

        /// <summary>
        /// Creates a new queue with the given queue name
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public async Task<bool> CreateQueueAsync(string queueName)
        {
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            // Create the queue if it doesn't already exist
            return await queue.CreateIfNotExistsAsync();
        }

        /// <summary>
        /// Deletes the queue with the given queue name if exists
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public async Task<bool> DeleteQueue(string queueName)
        {
            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            // Delete the queue.
            return await queue.DeleteIfExistsAsync();
        }

        #region Insert Message
        /// <summary>
        /// Inserts a new message to the queue with given string content
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task InsertMessageAsync(string queueName, string content)
        {
            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            // Create a message and add it to the queue.
            CloudQueueMessage message = new CloudQueueMessage(content);
            await queue.AddMessageAsync(message);
        }

        /// <summary>
        /// Inserts a new message to the queue with given byte content
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task InsertMessageAsync(string queueName, byte[] content)
        {
            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            // Create a message and add it to the queue.
            CloudQueueMessage message = new CloudQueueMessage(content);
            await queue.AddMessageAsync(message);
        }

        /// <summary>
        /// Inserts a new message to the queue with given pop receiptId and message Id
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="messageId"></param>
        /// <param name="popReceipt"></param>
        /// <returns></returns>
        public async Task InsertMessageAsync(string queueName, string messageId, string popReceipt)
        {
            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            // Create a message and add it to the queue.
            CloudQueueMessage message = new CloudQueueMessage(messageId, popReceipt);
            await queue.AddMessageAsync(message);
        }
        #endregion

        #region Update Message
        /// <summary>
        /// Update the content of the first message with given string content
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task UpdateMessage(string queueName, string content)
        {
            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            // Get the message from the queue and update the message contents.
            CloudQueueMessage message = await queue.GetMessageAsync();
            message.SetMessageContent(content);
            queue.UpdateMessage(message,
                TimeSpan.FromSeconds(0.0),  // Make it visible immediately.
                MessageUpdateFields.Content | MessageUpdateFields.Visibility);
        }

        /// <summary>
        /// Update the content of the first message with given byte content
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task UpdateMessage(string queueName, byte[] content)
        {
            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            // Get the message from the queue and update the message contents.
            CloudQueueMessage message = await queue.GetMessageAsync();
            message.SetMessageContent(content);
            queue.UpdateMessage(message,
                TimeSpan.FromSeconds(0.0),  // Make it visible immediately.
                MessageUpdateFields.Content | MessageUpdateFields.Visibility);
        }
        #endregion

        #region Get Message
        /// <summary>
        /// Gets the single message from the given queue
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public async Task<CloudQueueMessage> GetMessage(string queueName)
        {
            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            // Get the message from the queue 
            return await queue.GetMessageAsync();
        }

        /// <summary>
        /// Gets the messages of given count from queue 
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="messageCount"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CloudQueueMessage>> GetMessage(string queueName, int messageCount)
        {
            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            // Get the message from the queue 
            return await queue.GetMessagesAsync(messageCount);
        }
        #endregion

        /// <summary>
        /// Gets peek at the next message on the queue as string
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public async Task<string> PeekMessageAsString(string queueName)
        {
            // Retrieve a reference to a queue
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            // Peek at the next message
            CloudQueueMessage peekedMessage = await queue.PeekMessageAsync();
            return peekedMessage.AsString;
        }

        /// <summary>
        /// Gets peek at the next message on the queue as byte array
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public async Task<byte[]> GetPeekMessageAsByteArrayAsync(string queueName)
        {
            // Retrieve a reference to a queue
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            // Peek at the next message
            CloudQueueMessage peekedMessage = await queue.PeekMessageAsync();
            return peekedMessage.AsBytes;
        }

        /// <summary>
        /// Deletes the given queue 
        /// </summary>
        /// <param name="queueName"></param>
        public async void DeleteQueueAsync(string queueName)
        {
            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            // Delete the queue.
            await queue.DeleteAsync();
        }

        /// <summary>
        /// Gets the approximate queue count
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public async Task<int> GetQueueCountAsync(string queueName)
        {
            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            // Fetch the queue attributes.
            await queue.FetchAttributesAsync();
            // Retrieve the cached approximate message count.
            var cachedMessageCount = queue.ApproximateMessageCount;
            return cachedMessageCount == null ? 0 : (int)cachedMessageCount;
        }
    }
}
