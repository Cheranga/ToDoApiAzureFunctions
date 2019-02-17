using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using ToDoApi.InMemory;

namespace ToDoApi.Functions.Cosmos
{
    public static class GetAllTodosFunction
    {
        [FunctionName("Cosmos_GetAllTodos")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo4")]
            HttpRequest req,
            [CosmosDB(
                databaseName: "tododb",
                collectionName: "tasks",
                ConnectionStringSetting = "CosmosDbConnection",
                SqlQuery = "SELECT * FROM c order by c._ts desc")]
            IEnumerable<ToDo> toDoItems,
            ILogger log)
        {
            log.LogInformation("Getting all todos from cosmos db");

            var tasks = new List<ToDo>(toDoItems);

            if (tasks.Any())
            {
                return new OkObjectResult(tasks);
            }

            return new EmptyResult();
        }
    }
}
