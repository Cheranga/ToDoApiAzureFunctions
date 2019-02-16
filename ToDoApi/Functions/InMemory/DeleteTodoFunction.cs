using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ToDoApi.DTO;
using ToDoApi.InMemory;

namespace ToDoApi.Functions.InMemory
{
    public static class DeleteTodoFunction
    {
        [FunctionName("InMemory_DeleteTodo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "todo")]
            HttpRequest request,
            ILogger logger)
        {
            logger.LogInformation("Deleting todo item");

            var todoContent = await new StreamReader(request.Body).ReadToEndAsync();
            var todoItem = JsonConvert.DeserializeObject<TodoDeleteDto>(todoContent);

            var itemToDelete = InMemoryToDoCollection.Items.FirstOrDefault(x => x.Id == todoItem?.Id);
            if (itemToDelete == null)
            {
                return new NotFoundResult();
            }

            InMemoryToDoCollection.Items.Remove(itemToDelete);

            return new OkResult();
        }
    }
}