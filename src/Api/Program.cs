using Authentication;
using Carter;
using Core.Application.Exceptions.Handlers;
using Core.Application.Extensions;
using Job;
using JobApplication;
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
var jobApplicationAssembly = typeof(JobApplicationModule).Assembly;
var authAssembly = typeof(AuthenticationModule).Assembly;

builder.Services.AddCarterWithAssemblies(
    jobAssembly,
    jobApplicationAssembly,
    authAssembly
);

builder.Services.AddMediatRWithAssemblies(
    jobAssembly,
    jobApplicationAssembly,
    authAssembly
);

builder.Services.AddAuthorization();

builder.Services
    .AddJobModule(builder.Configuration)
    .AddJobApplicationModule(builder.Configuration)
    .AddAuthenticationModule(builder.Configuration)
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(c =>
        {
            // Add JWT authentication to Swagger
            c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        }
    );

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseSerilogRequestLogging();
app.UseExceptionHandler(_ => { });
app.UseAuthentication();
app.UseAuthorization();
app.MapCarter();

// Configure middleware extensions for job, application and authentication modules.
app
    .UseJobModule()
    .UseAuthenticationModule()
    .UseJobApplicationModule();

app.Run();
