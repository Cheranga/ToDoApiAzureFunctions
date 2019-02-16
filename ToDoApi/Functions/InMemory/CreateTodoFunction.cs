using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ToDoApi.DTO;
using ToDoApi.InMemory;

namespace ToDoApi.Functions.InMemory
{
    public static class CreateTodoFunction
    {
        [FunctionName("InMemory_CreateTodo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todo")]
            HttpRequest request,
            ILogger logger)
        {
            logger.LogInformation("Creating a todo item");

            var todoContent = await new StreamReader(request.Body).ReadToEndAsync();
            var todoTask = JsonConvert.DeserializeObject<ToDoCreateDto>(todoContent);

            var task = new ToDo
            {
                Description = todoTask.Description
            };

            InMemoryToDoCollection.Items.Add(task);

            return new OkObjectResult(todoTask);
        }
    }
}