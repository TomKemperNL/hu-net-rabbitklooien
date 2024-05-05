using Teachers.Inno.HU;
using Teachers.Inno.HU.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddTransient(s => new TeacherContext());
builder.Services.AddTransient(s => s.GetService<TeacherContext>()!.Teachers);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    var ctx = app.Services.GetService<TeacherContext>();
    ctx.Database.EnsureDeleted();
    ctx.Database.EnsureCreated();

    ctx.Teachers.Add(new Teacher("Bob", "bob@hu.nl"));
    ctx.Teachers.Add(new Teacher("Tom", "tom@hu.nl"));
    ctx.SaveChanges();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

