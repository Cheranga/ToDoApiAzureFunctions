namespace ToDoApi.DTO
{
    public class TodoUpdateDto
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}