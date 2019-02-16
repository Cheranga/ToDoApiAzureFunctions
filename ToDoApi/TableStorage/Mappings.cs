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
    }
}