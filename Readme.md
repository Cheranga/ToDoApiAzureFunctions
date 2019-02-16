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

---

### Blob Storage

- [ ] Get all todos
- [ ] Get todo by id
- [ ] Create a todo
- [ ] Update a todo
- [ ] Delete a todo

---

### Cosmos DB Storage

- [ ] Get all todos
- [ ] Get todo by id
- [ ] Create a todo
- [ ] Update a todo
- [ ] Delete a todo