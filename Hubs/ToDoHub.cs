namespace SignalRTodoApi.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using SignalRTodoApi.Models;
    using SignalRTodoApi.Models.DAL;

    public class ToDoHub : Hub
    {
        // NOTE: Client will make request all through the Controller !
        // so we dont need this Hub functions below any more, just leave it blank !
        // this Hub is just for initialization & Program.cs deps injection purpose





        // private static List<ToDoItem> toDoItems = new List<ToDoItem>();
        // private readonly ToDoContext _context;
        // public ToDoHub(ToDoContext context)
        // {
        //     _context = context;
        // }


        // public async Task<List<ToDoItem>> GetToDoItems()
        // {
        //     return await Task.FromResult(_context.ToDoItems.ToList());
        // }

        // // client will invoke this
        // public async Task AddToDoItem(ToDoItem item)
        // {
        //     item.Id = Guid.NewGuid().ToString();
        //     _context.ToDoItems.Add(item);
        //     await _context.SaveChangesAsync();

            
        //     await Clients.All.SendAsync("ReceiveUpdate", await GetToDoItems());
        // }

        // public async Task UpdateToDoItem(string id, ToDoItem item)
        // {
        //     var existingItem = await _context.ToDoItems.FindAsync(id);
        //     if (existingItem != null)
        //     {
        //         existingItem.Text = item.Text;
        //         existingItem.IsCompleted = item.IsCompleted;
        //         await Clients.All.SendAsync("ReceiveUpdate", await GetToDoItems());
        //     }
        // }

        // public async Task DeleteToDoItem(string id)
        // {
        //     var itemToRemove = await _context.ToDoItems.FindAsync(id);
        //     if (itemToRemove != null)
        //     {
        //         _context.ToDoItems.Remove(itemToRemove);
        //         await _context.SaveChangesAsync();
        //         await Clients.All.SendAsync("ReceiveUpdate", await GetToDoItems());
        //     }
        // }
    }
}