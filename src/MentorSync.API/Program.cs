using MentorSync.API;
using MentorSync.ServiceDefaults;
using MentorSync.SharedKernel.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(SwaggerConfiguration.Configure);

builder.Services.AddSerilog((_, lc) => lc.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddExceptionHandling();

builder.Services.AddCustomCorsPolicy();

builder.AddAplicationModules();

var app = builder.Build();

app.UseExceptionHandler();

app.UseCors("All");

app.MapDefaultEndpoints();

app.UseSwagger();
app.UseSwaggerUI();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapEndpoints();

app.Run();
