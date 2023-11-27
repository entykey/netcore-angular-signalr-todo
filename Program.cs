using Microsoft.EntityFrameworkCore;
using SignalRTodoApi.Hubs;
using SignalRTodoApi.Models.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddDbContext<ToDoContext>(opt => opt.UseInMemoryDatabase("ToDoList"));
builder.Services.AddSignalR();
builder.Services.AddDbContext<ToDoContext>(opt => opt.UseInMemoryDatabase("ToDoList"));
builder.Services.AddSignalR();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder
                .AllowCredentials()
                .WithOrigins(
                    "https://localhost:4200")
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseCors("AllowAllOrigins");
// Put this "UseCors" before "UseMvc" or else it wont permit any Origins !
// this one UseCors code solve all the CORS issues !! (preference: https://briancaos.wordpress.com/2022/10/03/net-api-cors-response-to-preflight-request-doesnt-pass-access-control-check-no-access-control-allow-origin-header-net-api/)
app.UseCors(CorsBuilder =>
{
    CorsBuilder
       //.AllowAnyOrigin()  // .net doesnt allow 'AllowAnyOrigin' together with 'AllowCredentials'
       .WithOrigins("http://localhost:4200", "https://localhost:4200") // set your ClientApp origins here !!
       .SetIsOriginAllowedToAllowWildcardSubdomains()
       .AllowAnyHeader()
       .AllowCredentials()
       .WithMethods("GET", "PUT", "POST", "DELETE", "OPTIONS")
       .SetPreflightMaxAge(TimeSpan.FromSeconds(3600)); // TimeSpan.FromMinutes(60)
});

app.UseAuthorization();

app.MapControllers();


app.MapHub<ToDoHub>("/todoHub");

app.Run();

