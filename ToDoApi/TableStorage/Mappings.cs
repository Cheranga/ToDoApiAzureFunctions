using ToDoApi.InMemory;

namespace ToDoApi.TableStorage
{
    public static class Mappings
    {
        public static ToDo ToTodo(this TodoTableEntity tableEntity)
        {
            return new ToDo
            {
                Id = tableEntity.RowKey,
                Description = tableEntity.Description,
                IsCompleted = tableEntity.IsCompleted,
                CreatedTime = tableEntity.CreatedTime
            };
        }

        public static TodoTableEntity ToTableEntity(this ToDo todo)
        {
            return new TodoTableEntity
            {
                RowKey = todo.Id,
                PartitionKey = "todo",
                CreatedTime = todo.CreatedTime,
                Description = todo.Description,
                IsCompleted = todo.IsCompleted
            };
        }
    }
}