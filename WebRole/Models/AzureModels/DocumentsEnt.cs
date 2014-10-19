using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole.Models.AzureModels
{
    public class DocumentsEnt : TableEntity
    {
        public DocumentsEnt(string Id)
        {
            this.PartitionKey = Id;
            this.RowKey = Id;
        }

        public DocumentsEnt() { }

        public string DocumentExplanation { get; set; }
    }
}