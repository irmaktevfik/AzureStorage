using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebRoleProject.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            AzureStorageRepository.TableRepository rep = new AzureStorageRepository.TableRepository();
            var result = await rep.CreateTableAsync("test");

            //var data = await rep.CreateTableAsync("DocumentEntity");
            //var data1 = await rep.InsertAsync<WebRoleProject.Models.DocumentEntity>(new Models.DocumentEntity("1", "1") { DocumentDescription = "test" }, "DocumentEntity");
            //var updateData = await rep.InsertOrReplaceSingleAsync<WebRoleProject.Models.DocumentEntity>("DocumentEntity", "1", "1", new Models.DocumentEntity("1", "1") { DocumentDescription = "test1" }, new List<string>() { "DocumentDescription" });

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(string a)
        {
            if (Request.Files != null)
                if (Request.Files.Count != 0)
                {
                    AzureStorageRepository.BlobRepository rep = new AzureStorageRepository.BlobRepository();
                    await rep.CreateContainerAsync("documents", true);

                    #region uploaded filedetails
                    byte[] buf = new byte[Request.Files[0].ContentLength];
                    Request.Files[0].InputStream.Read(buf, 0, Request.Files[0].ContentLength);
                    Stream stream = new MemoryStream(buf);
                    var dataname = Request.Files[0].FileName;
                    #endregion
                    //add file
                    await rep.UploadBlobAsync(stream, Request.Files[0].FileName, "documents");

                    var alldatauploaded = rep.GetListBlobs("documents");
                    await rep.DeleteBlobAsync("documents", "679603ebook.pdf");
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