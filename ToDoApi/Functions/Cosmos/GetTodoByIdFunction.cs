using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ToDoApi.InMemory;

namespace ToDoApi.Functions.Cosmos
{
    public static class GetTodoByIdFunction
    {
        [FunctionName("Cosmos_GetTodoById")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo4/{id}")]
            HttpRequest request,
            [CosmosDB(databaseName: "tododb", collectionName: "tasks", ConnectionStringSetting = "CosmosDbConnection", Id = "{id}")]
            ToDo todoItem,
            ILogger logger)
        {
            logger.LogInformation("Getting todo item by id");

            if (todoItem == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(todoItem);
        }
    }
}