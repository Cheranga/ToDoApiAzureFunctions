using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public static class GetAllTodosFunction
    {
        [FunctionName("BlobStorage_GetAllTodos")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo3")]
            HttpRequest request,
            [Blob("todos", Connection = "AzureWebJobsStorage")]
            CloudBlobContainer container,
            ILogger logger)
        {
            logger.LogInformation("Getting all todos from the blob container");

            await container.CreateIfNotExistsAsync();

            var blobSegments = await container.ListBlobsSegmentedAsync(null);

            var downloadBlobTasks = blobSegments.Results.Select(x =>
            {
                var blob = container.GetBlockBlobReference(x.Uri.Segments.Last());
                return blob.DownloadTextAsync();
            }).ToList();

            var todos = new List<ToDo>();

            await Task.WhenAll(downloadBlobTasks).ContinueWith(task =>
            {
                var downloadedContents = task.Result;
                todos.AddRange(downloadedContents.Select(JsonConvert.DeserializeObject<ToDo>));
            });

            return new OkObjectResult(todos);
        }
    }
}
