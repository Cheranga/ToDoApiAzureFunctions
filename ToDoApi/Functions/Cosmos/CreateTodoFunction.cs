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

namespace ToDoApi.Functions.Cosmos
{
    public static class CreateTodoFunction
    {
        [FunctionName("Cosmos_CreateTodo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todo4")]
            HttpRequest request,
            [CosmosDB(databaseName: "tododb", collectionName: "tasks", ConnectionStringSetting = "CosmosDbConnection")]
            IAsyncCollector<object> collection,
            ILogger logger)
        {
            logger.LogInformation("Creating a todo");

            var todoContent = await new StreamReader(request.Body).ReadToEndAsync();
            var createTodoDto = JsonConvert.DeserializeObject<ToDoCreateDto>(todoContent);
            var todo = new ToDo {Description = createTodoDto.Description};
            //
            // A dynamic object needs to be created otherwise we end up having an object with two properties of "Id" and "id".
            // the "id" is an auto-generated one from cosmos db.
            //
            var todoItem = new
            {
                id = todo.Id,
                todo.Description,
                todo.CreatedTime,
                todo.IsCompleted
            };

            await collection.AddAsync(todoItem);

            return new OkObjectResult(todoItem);
        }
    }
}