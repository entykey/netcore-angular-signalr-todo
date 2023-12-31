﻿namespace SignalRTodoApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using SignalRTodoApi.Hubs;
    using SignalRTodoApi.Models;
    using SignalRTodoApi.Models.DAL;


    [ApiController]
    [Route("api/[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly IHubContext<ToDoHub> _hubContext;
        private readonly ToDoContext _context;

        public ToDoController(ToDoContext context, IHubContext<ToDoHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IEnumerable<ToDoItem>> Get()
        {
            return _context.ToDoItems.ToList();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ToDoItem item)
        {
            item.Id = Guid.NewGuid().ToString();
            _context.ToDoItems.Add(item);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("ReceiveUpdate", _context.ToDoItems.ToList());

            return Ok(item);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ToDoItem item)
        {
            var existingItem = await _context.ToDoItems.FindAsync(item.Id);

            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Text = item.Text;
            existingItem.IsCompleted = item.IsCompleted;

            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("ReceiveUpdate", _context.ToDoItems.ToList());

            return Ok(existingItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var item = await _context.ToDoItems.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            _context.ToDoItems.Remove(item);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("ReceiveUpdate", _context.ToDoItems.ToList());

            return Ok(item);
        }
    }

}

