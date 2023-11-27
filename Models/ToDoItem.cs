namespace SignalRTodoApi.Models
{
    using System;

    public class ToDoItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Text { get; set; }
        public bool IsCompleted { get; set; }
    }

}

