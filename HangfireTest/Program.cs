using Hangfire;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHangfire(x => x.UseSqlServerStorage(@"Server=127.0.0.1;Database=HangfireTestDb;User Id=sa;Password=P@sword123!;"));
builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard();
RecurringJob.AddOrUpdate( "database-update",() => Console.WriteLine("Database updated"), Cron.Minutely);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();