using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace ToDoApi.Functions.Cosmos
{
    public static class DeleteTodoFunction
    {
        [FunctionName("Cosmos_DeleteTodo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "todo4/{id}")]
            HttpRequest request,
            string id,
            [CosmosDB(ConnectionStringSetting = "CosmosDbConnection")]
            DocumentClient documentClient,
            ILogger logger)
        {
            logger.LogInformation("Deleting a todo item");

            var uri = UriFactory.CreateDocumentCollectionUri("tododb", "tasks");
            var existingDocument = documentClient.CreateDocumentQuery(uri).Where(x => x.Id == id).AsEnumerable().FirstOrDefault();

            if (existingDocument == null)
            {
                return new NotFoundResult();
            }

            await documentClient.DeleteDocumentAsync(existingDocument.SelfLink);

            return new OkResult();
        }
    }
}