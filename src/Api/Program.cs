using Carter;
using Core.Application.Exceptions.Handlers;
using Core.Application.Extensions;
using Job;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration)
);

// Load environments variables from .env file
DotNetEnv.Env.Load();
DotNetEnv.Env.TraversePath().Load();

// Add services to the container.
// Register Carter and MediatR Assemblies
var jobAssembly = typeof(JobModule).Assembly;
// var jobApplicationAssembly = typeof(JobApplicationModule).Assembly;
// var authAssembly = typeof(AuthenticationModule).Assembly;

builder.Services.AddCarterWithAssemblies(
    jobAssembly
);

builder.Services.AddMediatRWithAssemblies(
    jobAssembly 
);

builder.Services.AddAuthorization();

builder.Services
    .AddJobModule(builder.Configuration);
    // .AddAuthenticationModule(builder.Configuration)
    // .AddJobApplicationModule(builder.Configuration);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

app.MapCarter();
app.UseSerilogRequestLogging();
app.UseExceptionHandler(_ => { });
app.UseAuthentication();
app.UseAuthorization();

// Configure middleware extensions for job, application and authentication modules.
app
    .UseJobModule();
    // .UseAuthenticationModule()
    // .UseJobApplicationModule();

app.Run();
