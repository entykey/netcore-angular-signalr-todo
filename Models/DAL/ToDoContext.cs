namespace SignalRTodoApi.Models.DAL
{
    using Microsoft.EntityFrameworkCore;


    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options) { }

        public DbSet<ToDoItem> ToDoItems { get; set; }
    }
}
