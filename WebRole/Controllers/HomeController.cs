using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebRole.Models;
using WebRole.Models.AzureModels;

namespace WebRole.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            AzureStorageRepository.TableRepository rep = new AzureStorageRepository.TableRepository();
            rep.CreateTableAsync("documents");
            //// Retrieve the storage account from the connection string.
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            //// Create the table client.
            //CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            //// Create the table if it doesn't exist.
            //CloudTable table = tableClient.GetTableReference("documents");
            //table.CreateIfNotExists();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(HomeModel hModel)
        {
            if (ModelState.IsValid)
            {
                Models.AzureModels.DocumentsEnt ent = new DocumentsEnt("1");
                ent.DocumentExplanation = hModel.DocumentName;

                AzureStorageRepository.TableRepository rep = new AzureStorageRepository.TableRepository();
                await rep.InsertAsync<Models.AzureModels.DocumentsEnt>(ent,"documents");
                //// Retrieve the storage account from the connection string.
                //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

                //Models.AzureModels.DocumentsEnt ent = new DocumentsEnt("1");
                //ent.DocumentExplanation = hModel.DocumentName;

                //// Create the TableOperation that inserts the customer entity.
                //TableOperation insertOperation = TableOperation.Insert(ent);

                //CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                //CloudTable table = tableClient.GetTableReference("documents");
                //// Execute the insert operation.
                //table.Execute(insertOperation);
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}