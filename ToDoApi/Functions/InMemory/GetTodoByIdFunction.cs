using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ToDoApi.InMemory;

namespace ToDoApi.Functions.InMemory
{
    public static class GetTodoByIdFunction
    {
        [FunctionName("InMemory_GetTodoItemById")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo/{id}")]
            HttpRequest request,
            string id,
            ILogger logger)
        {
            logger.LogInformation($"Getting the todo item by id:{id}");

            var task = InMemoryToDoCollection.Items.FirstOrDefault(x => x.Id == id);
            if (task == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(task);
        }
    }
}