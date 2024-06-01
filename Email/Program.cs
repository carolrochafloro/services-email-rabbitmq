using Email;
using dotenv.net;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

DotEnv.Load();

var host = builder.Build();
host.Run();
