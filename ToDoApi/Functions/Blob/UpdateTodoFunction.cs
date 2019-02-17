using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using ToDoApi.DTO;

namespace ToDoApi.Functions.Blob
{
    public static class UpdateTodoFunction
    {
        [FunctionName("BlobStorage_UpdateTodo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "todo3/{id}")]
            HttpRequest request,
            [Blob("todos/{id}.json", Connection = "AzureWebJobsStorage")]
            CloudBlockBlob blob,
            ILogger logger)
        {
            if (blob == null)
            {
                return new NotFoundResult();
            }

            var updatedContent = await new StreamReader(request.Body).ReadToEndAsync();
            var updatedTodo = JsonConvert.DeserializeObject<TodoUpdateDto>(updatedContent);

            var existingTodoContent = await blob.DownloadTextAsync();
            var existingTodo = JsonConvert.DeserializeObject<TodoUpdateDto>(existingTodoContent);

            if (!string.IsNullOrEmpty(updatedTodo.Description))
            {
                existingTodo.Description = updatedTodo.Description;
            }

            existingTodo.IsCompleted = updatedTodo.IsCompleted;

            await blob.UploadTextAsync(JsonConvert.SerializeObject(existingTodo));

            return new OkObjectResult(existingTodo);
        }
    }
}
