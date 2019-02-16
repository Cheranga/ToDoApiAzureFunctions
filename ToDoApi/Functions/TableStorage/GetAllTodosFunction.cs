using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace ToDoApi.Functions.TableStorage
{
    public static class GetAllTodosFunction
    {
        [FunctionName("TableStorage_GetAllTodos")]
        public static void Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo2")]HttpRequest request,
            [Table("todos","todo",Connection = "AzureWebJobsStorage")]CloudTable table,
            ILogger log)
        {
            log.LogInformation("Getting all todos from the table storage");


        }
    }
}
