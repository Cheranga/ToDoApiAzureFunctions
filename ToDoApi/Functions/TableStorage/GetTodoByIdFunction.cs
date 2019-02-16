using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ToDoApi.TableStorage;

namespace ToDoApi.Functions.TableStorage
{
    public static class GetTodoByIdFunction
    {
        [FunctionName("TableStorage_GetTodoById")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo2/{id}")]
            HttpRequest request,
            [Table("todos","todo","{id}", Connection = "AzureWebJobsStorage")]TodoTableEntity todoTableEntity,
            ILogger logger)
        {
            logger.LogInformation($"Getting todo item by id");

            if (todoTableEntity == null)
            {
                return new NotFoundResult();
            }

            var todo = todoTableEntity.ToTodo();
            return new OkObjectResult(todo);
        }
    }
}