using MentorSync.ServiceDefaults;
using MentorSync.Users;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddUsersModule(builder);

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.Run();
