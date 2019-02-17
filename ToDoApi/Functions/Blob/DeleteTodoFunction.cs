using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ToDoApi.Functions.Blob
{
    public static class DeleteTodoFunction
    {
        [FunctionName("BlobStorage_DeleteTodo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "todo3/{id}")]
            HttpRequest request,
            [Blob("todos/{id}.json", Connection = "AzureWebJobsStorage")]
            CloudBlockBlob blob,
            ILogger logger)
        {
            logger.LogInformation("Deleting todo item");

            var status = await blob.DeleteIfExistsAsync();

            if (status)
            {
                return new OkResult();
            }

            return new NotFoundResult();
        }
    }
}