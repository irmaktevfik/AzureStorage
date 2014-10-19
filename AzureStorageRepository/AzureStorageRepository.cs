using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageRepository
{
    public class AzureStorageRepository : RoleEntryPoint
    {        
        internal CloudStorageAccount storageAccount;

        public AzureStorageRepository()
        {
            // Retrieve the storage account from the connection string.
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("AzureStorageConnectionString"));
        }

        public override bool OnStart()
        {

            return base.OnStart();
        }


    }
}
