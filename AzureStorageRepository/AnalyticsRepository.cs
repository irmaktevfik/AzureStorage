using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Analytics;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections;
using System.Collections.Generic;
using System;


namespace AzureStorageRepository
{
    public class AnalyticsRepository : AzureStorageRepository
    {
        CloudAnalyticsClient analyticsClient;
        StorageService service;

        public AnalyticsRepository(StorageService serviceType)
        {
            analyticsClient = base.storageAccount.CreateCloudAnalyticsClient();
            service = serviceType;
        }

        /// <summary>
        /// Gets the hourly metrics table for the specified storage service.
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public CloudTable GetHourlyMetricsTable()
        {
            return analyticsClient.GetHourMetricsTable(service);
        }

        /// <summary>
        /// Gets the hourly metrics table for the specified storage service.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public CloudTable GetHourlyMetricsTable(StorageLocation location)
        {
            return analyticsClient.GetHourMetricsTable(service, location);
        }

        /// <summary>
        /// Returns the directory for the logs of the given service
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public CloudBlobDirectory GetLogDirectory()
        {
            return analyticsClient.GetLogDirectory(service);
        }

        /// <summary>
        /// Gets the minute metrics table for the specified storage service.
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public CloudTable GetMinuteMetricsTable()
        {
            return analyticsClient.GetMinuteMetricsTable(service);
        }

        /// <summary>
        /// Gets the minute metrics table for the specified storage service for the given location
        /// </summary>
        /// <param name="service"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public CloudTable GetMinuteMetricsTable(StorageLocation location)
        {
            return analyticsClient.GetMinuteMetricsTable(service, location);
        }

        /// <summary>
        /// Returns the Log records for the service
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public IEnumerable<LogRecord> ListLogRecords()
        {
            return analyticsClient.ListLogRecords(service);
        }

        /// <summary>
        /// Returns the Log records for the service between given date range
        /// </summary>
        /// <param name="service"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public IEnumerable<LogRecord> ListLogRecords(DateTimeOffset startTime, DateTimeOffset? endTime)
        {
            return analyticsClient.ListLogRecords(service, startTime, endTime);
        }

        /// <summary>
        /// Returns log blobs for given service
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public IEnumerable<ICloudBlob> ListLogs()
        {
            return analyticsClient.ListLogs(service);
        }

        /// <summary>
        /// Returns log blobs for given service and daterange
        /// </summary>
        /// <param name="service"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public IEnumerable<ICloudBlob> ListLogs(DateTimeOffset startTime, DateTimeOffset? endTime)
        {
            return analyticsClient.ListLogs(service, startTime, endTime);
        }
    }
}
