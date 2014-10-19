using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRoleProject.Models
{
    public class DocumentEntity : TableEntity
    {
        public DocumentEntity(string partitionKey, string rowKey)
        {
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
        }

        public DocumentEntity()
        {

        }

        public string DocumentDescription { get; set; }
    }
}