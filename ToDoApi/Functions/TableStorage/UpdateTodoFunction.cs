using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using ToDoApi.DTO;
using ToDoApi.TableStorage;

namespace ToDoApi.Functions.TableStorage
{
    public static class UpdateTodoFunction
    {
        [FunctionName("TableStorage_UpdateTodo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "todo2/{id}")]
            HttpRequest request,
            string id,
            [Table("todos", "todo", Connection = "AzureWebJobsStorage")]CloudTable table,
            [Table("todos", "todo", "{id}", Connection = "AzureWebJobsStorage")]TodoTableEntity tableEntity,
            ILogger logger)
        {
            logger.LogInformation("Updating todo");

            if (tableEntity == null)
            {
                return new NotFoundResult();
            }
            //
            // NOTE:
            // Instead of using binding to get the entity, we could have get the item through code as shown below.
            // But why do we want to do it by ourselves if the bindings does it for us?! :D
            //
            var retrieveOperation = TableOperation.Retrieve<TodoTableEntity>("todo", id);
            var retrieveOperationResult = await table.ExecuteAsync(retrieveOperation);
            var todoToUpdate = retrieveOperationResult.Result as TodoTableEntity;

            var todoContent = await new StreamReader(request.Body).ReadToEndAsync();
            var updatedTodoDto = JsonConvert.DeserializeObject<TodoUpdateDto>(todoContent);
            if (updatedTodoDto == null)
            {
                return new BadRequestResult();
            }

            if (!string.IsNullOrEmpty(updatedTodoDto?.Description))
            {
                tableEntity.Description = updatedTodoDto.Description;
            }

            tableEntity.IsCompleted = updatedTodoDto.IsCompleted;

            var replaceOperation = TableOperation.Replace(tableEntity);
            var operation = await table.ExecuteAsync(replaceOperation);
            if (operation.HttpStatusCode == 204)
            {
                var todo = (operation.Result as TodoTableEntity).ToTodo();

                return new OkObjectResult(todo);
            }

            logger.LogError("Cannot update todo item");

            return new InternalServerErrorResult();
        }
    }
}