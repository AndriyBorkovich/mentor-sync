using MentorSync.API.Extensions;
using MentorSync.ServiceDefaults;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddEndpointsMetadata();

builder.Services.AddCustomSerilog(builder.Configuration);

builder.Services.AddExceptionHandling();

builder.Services.AddCustomCorsPolicy();

builder.AddAplicationModules();

var app = builder.Build();

app.UseExceptionHandler();

app.UseCors(CorsPolicyNames.All);

app.MapDefaultEndpoints();

app.UseCustomSwagger();

app.UseCustomSerilog();

app.UseHttpsRedirection();

app.UseAuth();

app.UseSession();

app.MapEndpoints();

app.Run();
