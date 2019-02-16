using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using ToDoApi.DTO;
using ToDoApi.InMemory;

namespace ToDoApi.Functions.Blob
{
    public static class CreateTodoFunction
    {
        [FunctionName("Blob_CreateTodo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todo3")]
            HttpRequest request,
            [Blob("todos", Connection = "AzureWebJobsStorage")]
            CloudBlobContainer blobContainer,
            ILogger logger)
        {
            logger.LogInformation("Creating todo in the blob");
            //
            // Create the blob container if it does not exist. This behaviour is different to table-storage. In there it seems that if the table does not exist,
            // it will create the table first before adding any content to it.
            //
            await blobContainer.CreateIfNotExistsAsync();

            var todoContent = await new StreamReader(request.Body).ReadToEndAsync();
            var todoCreateDto = JsonConvert.DeserializeObject<ToDoCreateDto>(todoContent);
            var todo = new ToDo {Description = todoCreateDto.Description};

            var blob = blobContainer.GetBlockBlobReference($"{todo.Id}.json");
            await blob.UploadTextAsync(JsonConvert.SerializeObject(todo));

            return new OkObjectResult(todo);
        }
    }
}