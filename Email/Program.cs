using dotenv.net;
using Email;
using Email.Context;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

DotEnv.Load();

string? connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

builder.Services.AddDbContext<EmailContext>(options =>
{
    options.UseSqlServer(connectionString);
});

var host = builder.Build();
host.Run();
