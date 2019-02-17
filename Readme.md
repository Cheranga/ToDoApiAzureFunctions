## TODO

### In-memory Storage

- [x] Get all todos
- [x] Get todo by id
- [x] Create a todo
- [x] Update a todo
- [x] Delete a todo

### Table Storage

- [x] Get all todos
- [x] Get todo by id
- [x] Create a todo
- [x] Update a todo
- [x] Delete a todo

```CSharp
// This will retrieve the object from the table storage
var retrieveOperation = TableOperation.Retrieve<TodoTableEntity>("todo", id);
var retrieveOperationResult = await table.ExecuteAsync(retrieveOperation);
var todoTableEntity = (TodoTableEntity)(retrieveOpertionResult.Result);

// Also you can use bindings to retrieve the object then and there
[FunctionName("TableStorage_UpdateTodo")]
public static async Task<IActionResult> Run(
    [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "todo2/{id}")]
    HttpRequest request,    
    [Table("todos", "todo", Connection = "AzureWebJobsStorage")]CloudTable table,
    [Table("todos", "todo", "{id}", Connection = "AzureWebJobsStorage")]TodoTableEntity tableEntity,
    ILogger logger)

```

* Check how to add a blob using the `IAsyncCollector`. So that you'll know multiple ways to do so.

---

### Blob Storage

- [x] Get all todos
- [x] Get todo by id
- [x] Create a todo
- [x] Update a todo
- [x] Delete a todo

* Getting an item from the blob storage can be done in two ways.

1. Use the input binding of `Blob`

```CSharp
[FunctionName("BlobStorage_GetTodoById")]
public static async Task<IActionResult> Run(
[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo3/{id}")]HttpRequest request,
string id,
[Blob("todos/{id}.json", Connection = "AzureWebJobsStorage")]string blobContent,
ILogger logger)
    {
        logger.LogInformation($"Getting todo item :{id}");

        if (string.IsNullOrEmpty(blobContent))
        {
            return new NotFoundResult();
        }

        var todo = JsonConvert.DeserializeObject<ToDo>(blobContent);
        return new OkObjectResult(todo);
    }
```

2. Bind the blob container itself and retrieve the content through code.

```CSharp
[FunctionName("BlobStorage_GetTodoById")]
public static async Task<IActionResult> Run(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo3/{id}")]HttpRequest request,
    string id,
    [Blob("todos", Connection = "AzureWebJobsStorage")]CloudBlobContainer container,
    ILogger logger)
    {
        logger.LogInformation($"Getting todo item :{id}");

        var blob = container.GetBlockBlobReference($"{id}.json");
        if (blob == null)
        {
            return new NotFoundResult();
        }

        var blobContent = await blob.DownloadTextAsync();
        var todo = JsonConvert.DeserializeObject<ToDo>(blobContent);

        return new OkObjectResult(todo);
    }
``` 

---

### Cosmos DB Storage

- [x] Get all todos
- [x] Get todo by id
- [x] Create a todo
- [x] Update a todo
- [x] Delete a todo

*For reference refer these examples on how to use cosmos db https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-cosmosdb-v2#input---c-examples*

* When creating a todo item, had to use a `dynamic` object. This is because otherwise if I use my class (`Todo`) the document
object will endup having two properties (`Id`, and `id`). To avoid this we need to create a dynamic object with 
the required properties and then add it as a document.

* When retrieving a document using the `DocumentClient` instance since all the LINQ operations are not supported, had to do some
`AsEnumerable` gymnastics in there (refer the `UpdateTodoFunction.cs` in cosmos section).

