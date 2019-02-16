using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace ToDoApi.TableStorage
{
    public class TodoTableEntity : TableEntity
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    }
}