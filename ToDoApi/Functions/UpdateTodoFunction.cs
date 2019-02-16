using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ToDoApi.InMemory;

namespace ToDoApi.Functions
{
    public static class UpdateTodoFunction
    {
        [FunctionName("InMemory_UpdateTodo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "todo")]
            HttpRequest request,
            ILogger logger)
        {
            logger.LogInformation("Updating a todo task");

            var todoContent = await new StreamReader(request.Body).ReadToEndAsync();
            var updatedTodo = JsonConvert.DeserializeObject<TodoUpdateDto>(todoContent);

            var taskToUpdate = InMemoryToDoCollection.Items.FirstOrDefault(x => x.Id == updatedTodo.Id);

            if (taskToUpdate == null)
            {
                return new NotFoundResult();
            }

            if (!string.IsNullOrEmpty(updatedTodo?.Description))
            {
                taskToUpdate.Description = updatedTodo.Description;
            }

            taskToUpdate.IsCompleted = updatedTodo.IsCompleted;

            return new OkObjectResult(taskToUpdate);
        }
    }
}