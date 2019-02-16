using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using ToDoApi.TableStorage;

namespace ToDoApi.Functions.TableStorage
{
    public static class DeleteTodoFunction
    {
        [FunctionName("TableStorage_DeleteTodo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "todo2/{id}")]
            HttpRequest request,
            string id,
            [Table("todos", "todo", Connection = "AzureWebJobsStorage")]CloudTable table,
            [Table("todos", "todo", "{id}", Connection = "AzureWebJobsStorage")]TodoTableEntity tableEntity,
            ILogger logger)
        {
            logger.LogInformation("Deleting todo item");

            if (tableEntity == null)
            {
                return new NotFoundResult();
            }

            var deleteOperation = TableOperation.Delete(tableEntity);
            var deleteOperationResult = await table.ExecuteAsync(deleteOperation);
            if (deleteOperationResult.HttpStatusCode == 204)
            {
                return new OkResult();
            }

            logger.LogError("Cannot delete todo item");

            return new InternalServerErrorResult();

        }
    }
}