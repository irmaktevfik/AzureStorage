using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageRepository
{
    public class BlobRepository : AzureStorageRepository
    {
        CloudBlobClient blobClient;
        CloudBlobContainer blobContainer;

        public BlobRepository()
        {
            blobClient = storageAccount.CreateCloudBlobClient();
        }

        /// <summary>
        /// Creates a private container with the given name
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        public async Task<bool> CreateContainerAsync(string containerName, bool isPublic)
        {
            // Create the container if it doesn't exist.
            blobContainer = blobClient.GetContainerReference(containerName);
            if (isPublic)
            {
                var returnData = await blobContainer.CreateIfNotExistsAsync();
                if (returnData)
                    await blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                return returnData;
            }
            return await blobContainer.CreateIfNotExistsAsync();
        }

        public async Task<bool> DeleteContainerAsync(string containerName)
        {
            // Delete the container if it doesn't exist.
            blobContainer = blobClient.GetContainerReference(containerName);
            return await blobContainer.DeleteIfExistsAsync();
        }

        /// <summary>
        /// Uploads the given stream to the name of the blob. Please note that if blob exists, contents will be overriten.
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="blobNameToCreate"></param>
        /// <param name="containerName"></param>
        /// <returns></returns>
        public async Task UploadBlobAsync(Stream fileStream, string blobNameToCreate, string containerName)
        {
            // Retrieve reference the given container
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            // Retrieve reference to the blob
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobNameToCreate);

            // Create or overwrite blob with contents from parameter.
            await blockBlob.UploadFromStreamAsync(fileStream);
        }

        /// <summary>
        /// gets the collection of IListBlobItem in the given container holding CloudBlobContainer, CloudBlobDirectory, StorageUri and URI
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        public IEnumerable<IListBlobItem> GetListBlobs(string containerName)
        {
            // Retrieve reference the given container
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            // Get List of blobs is not async, so we need to wrap it with a Task to wait until finished
            return container.ListBlobs(null, false);
        }

        /// <summary>
        /// Download given blob and return filestream
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        /// <returns></returns>

        public async Task<FileStream> DownloadBlobAsStreamAsync(FileStream fileStream, string containerName, string blobName)
        {
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
            // Save blob contents to a filestream.
            await blockBlob.DownloadToStreamAsync(fileStream);
            return fileStream;
        }

        /// <summary>
        /// Download given blob as string and return filestream
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        /// <returns></returns>
        public async Task<string> DownloadBlobContentAsStringAsync(string containerName, string blobName)
        {
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);

            // Save blob contents to a filestream.
            using (MemoryStream mStream = new MemoryStream())
            {
                await blockBlob.DownloadToStreamAsync(mStream);
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
        }

        /// <summary>
        /// Download given blob as byte[]
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        /// <returns></returns>
        public async Task<byte[]> DownloadBlobContentAsByteArrayAsync(string containerName, string blobName)
        {
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);

            // Save blob contents to a MemoryStream.
            using (MemoryStream mStream = new MemoryStream())
            {
                await blockBlob.DownloadToStreamAsync(mStream);
                return mStream.ToArray();
            }
        }

        /// <summary>
        /// Deletes the given blob
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        /// <returns></returns>
        public async Task DeleteBlobAsync(string containerName, string blobName)
        {
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
            await blockBlob.DeleteAsync();
        }
    }
}
