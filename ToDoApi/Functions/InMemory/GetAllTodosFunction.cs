using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ToDoApi.InMemory;

namespace ToDoApi.Functions.InMemory
{
    public static class GetAllTodosFunction
    {
        [FunctionName("InMemory_GetAllTodos")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo")]
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Getting all todo items");

            return new OkObjectResult(InMemoryToDoCollection.Items);
        }
    }
}