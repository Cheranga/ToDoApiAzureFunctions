using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ToDoApi.DTO;

namespace ToDoApi.Functions.Cosmos
{
    public static class UpdateTodoFunction
    {
        [FunctionName("Cosmos_UpdateTodo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "todo4/{id}")]
            HttpRequest request,
            string id,
            [CosmosDB(ConnectionStringSetting = "CosmosDbConnection")]
            DocumentClient documentClient,
            ILogger logger)
        {
            logger.LogInformation("Updating a todo item");

            var updatedContent = await new StreamReader(request.Body).ReadToEndAsync();
            var updatedTodoDto = JsonConvert.DeserializeObject<TodoUpdateDto>(updatedContent);

            var uri = UriFactory.CreateDocumentCollectionUri("tododb", "tasks");
            var existingTodoDocument = documentClient.CreateDocumentQuery(uri).Where(x => x.Id == id).AsEnumerable().FirstOrDefault();

            if (existingTodoDocument == null)
            {
                return new NotFoundResult();
            }

            existingTodoDocument.SetPropertyValue("IsCompleted", updatedTodoDto.IsCompleted);

            if (!string.IsNullOrEmpty(updatedTodoDto.Description))
            {
                existingTodoDocument.SetPropertyValue("Description", updatedTodoDto.Description);
            }
            //
            // Persist the updates by replacing the document
            //
            await documentClient.ReplaceDocumentAsync(existingTodoDocument);

            return new OkObjectResult((dynamic)(existingTodoDocument));
        }
    }
}
