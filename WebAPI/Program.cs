using Application.DaoInterfaces;
using Application.Logic;
using Application.LogicInterfaces;
using DBConnection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
// builder.Configuration.AddEnvironmentVariables();


builder.Services.AddControllers();
builder.Services.AddScoped<IStoryDAO, StoryDAO>();
builder.Services.AddScoped<IDepartmentDAO, DepartmentDAO>();
builder.Services.AddScoped<IStoryLogic, StoryLogic>();
builder.Services.AddScoped<IDepartmentLogic, DepartmentLogic>();



var app = builder.Build();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials());

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();


app.MapControllers();
app.Urls.Add("http://0.0.0.0:80");



app.Run();