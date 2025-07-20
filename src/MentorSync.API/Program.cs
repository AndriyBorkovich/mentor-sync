using MentorSync.API.Extensions;
using MentorSync.ServiceDefaults;
using MentorSync.SharedKernel.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddEndpointsMetadata();

builder.Services.AddCustomSerilog(builder.Configuration);

builder.Services.ConfigureJsonOptions();

builder.Services.AddExceptionHandling();

builder.Services.AddGlobalRateLimiting();

builder.Services.AddCustomCorsPolicy();

builder.AddApplicationModules();

var app = builder.Build();

app.UseExceptionHandler();

app.UseCustomCorsPolicy();

app.MapDefaultEndpoints();

app.UseCustomSwagger();

app.UseCustomSerilog();

app.UseHttpsRedirection();

app.UseAuth();

app.UseRateLimiter();

app.UseAntiforgery();

app.UseSession();

app.MapEndpoints();

app.MapHubs();

await app.RunAsync();
