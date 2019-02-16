using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using ToDoApi.InMemory;

namespace ToDoApi.Functions.Blob
{
    public static class GetTodoByIdFunction
    {
        //[FunctionName("BlobStorage_GetTodoById")]
        //public static async Task<IActionResult> Run(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo3/{id}")]
        //    HttpRequest request,
        //    string id,
        //    [Blob("todos", Connection = "AzureWebJobsStorage")]
        //    CloudBlobContainer container,
        //    ILogger logger)
        //{
        //    logger.LogInformation($"Getting todo item :{id}");

        //    var blob = container.GetBlockBlobReference($"{id}.json");
        //    if (blob == null)
        //    {
        //        return new NotFoundResult();
        //    }

        //    var blobContent = await blob.DownloadTextAsync();
        //    var todo = JsonConvert.DeserializeObject<ToDo>(blobContent);

        //    return new OkObjectResult(todo);
        //}


        [FunctionName("BlobStorage_GetTodoById")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo3/{id}")]HttpRequest request,
            string id,
            [Blob("todos/{id}.json", Connection = "AzureWebJobsStorage")]string blobContent,
            ILogger logger)
        {
            logger.LogInformation($"Getting todo item :{id}");

            if (string.IsNullOrEmpty(blobContent))
            {
                return new NotFoundResult();
            }

            var todo = JsonConvert.DeserializeObject<ToDo>(blobContent);
            return new OkObjectResult(todo);
        }
    }
}