using CatOs.Infrastructure.Config.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.EnvironmentSetup(builder);
builder.Services.AddApplicationServices();
builder.Services.AddDataBase(builder.Configuration);
builder.Services.AddCorsPolicies();
//builder.Services.AddQuartzJobs();

var app = builder.Build();

builder.Services.AddApplicationConfig(app);

app.Run();
