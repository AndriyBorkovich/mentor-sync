var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.MentorSync_API>("MentorSyncApi");
builder.Build().Run();