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
using ToDoApi.TableStorage;

namespace ToDoApi.Functions.TableStorage
{
    public static class CreateTodoFunction
    {
        [FunctionName("TableStorage_CreateTodo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todo2")]
            HttpRequest request,
            [Table("todos", "todo", Connection = "AzureWebJobsStorage")]
            IAsyncCollector<TodoTableEntity> collection,
            ILogger logger)
        {
            logger.LogInformation("Creating a todo in table storage");

            var todoContent = await new StreamReader(request.Body).ReadToEndAsync();
            var todoDto = JsonConvert.DeserializeObject<ToDoCreateDto>(todoContent);
            var todo = new ToDo {Description = todoDto.Description};
            var tableEntity = todo.ToTableEntity();

            await collection.AddAsync(tableEntity);

            return new OkObjectResult(tableEntity.ToTodo());
        }
    }
}