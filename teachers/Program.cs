using HU.Inno.Teachers.Messaging;
using Teachers.Inno.HU;
using Teachers.Inno.HU.Controllers;
using Teachers.Inno.HU.Domain;

var builder = WebApplication.CreateBuilder(args);

var rabbitStartup = new RabbitStartup();
rabbitStartup.ConfigureQueues();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<TeacherDBContext>();

builder.Services.AddTransient(s => s.GetService<TeacherDBContext>()!.Teachers);
builder.Services.AddTransient<IUnitOfWork>(s => new DbContextUnitOfWork(s.GetService<TeacherDBContext>()!));
rabbitStartup.ConfigurePublishers(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var ctx = scope.ServiceProvider.GetService<TeacherDBContext>();
        ctx.Database.EnsureDeleted();
        ctx.Database.EnsureCreated();

        ctx.Teachers.Add(new Teacher("Bob", "bob@hu.nl"));
        ctx.Teachers.Add(new Teacher("Tom", "tom@hu.nl"));
        ctx.SaveChanges();
    }

    rabbitStartup.ConfigureSubscribers(app.Services);
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();