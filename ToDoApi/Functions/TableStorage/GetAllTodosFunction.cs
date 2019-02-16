using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using ToDoApi.TableStorage;

namespace ToDoApi.Functions.TableStorage
{
    public static class GetAllTodosFunction
    {
        [FunctionName("TableStorage_GetAllTodos")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo2")]HttpRequest request,
            [Table("todos","todo",Connection = "AzureWebJobsStorage")]CloudTable table,
            ILogger log)
        {
            log.LogInformation("Getting all todos from the table storage");

            var query = new TableQuery<TodoTableEntity>();
            var segment = await table.ExecuteQuerySegmentedAsync(query, null);
            var todos = segment.Results.Select(x => x.ToTodo());

            return new OkObjectResult(todos);
        }
    }
}
