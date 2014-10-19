using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageRepository.Helper
{
    public static class HttpStatusCodeHelper
    {
        /// <summary>
        /// Processes http status codes
        /// </summary>
        /// <param name="result"></param>
        /// <returns>true if successful</returns>
        public static bool GetResultFromTableResult(TableResult result)
        {
            //The request has succeeded
            if (result.HttpStatusCode == (int)HttpStatusCode.OK
                //The server has fulfilled the request but does not need to return an entity-body
                || result.HttpStatusCode == (int)HttpStatusCode.NoContent)
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Process batch results
        /// </summary>
        /// <param name="result"></param>
        /// <returns>Returns true if all batch successful</returns>
        public static bool GetResultFromTableListResult(IList<TableResult> result)
        {
            // you can go ahead and return list of results
            //List<TableResult> lst = new List<TableResult>();
            bool isSuccessful = true;

            foreach (var item in result)
            {
                //The request has succeeded
                if (item.HttpStatusCode != (int)HttpStatusCode.OK
                    //The server has fulfilled the request but does not need to return an entity-body
                    || item.HttpStatusCode != (int)HttpStatusCode.NoContent)
                {
                    isSuccessful = false;
                }
            }
            return isSuccessful;
        }
    }
}
