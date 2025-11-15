1. Ensure that all services are registered in IoC container
2. Go to specific module folder (e.g. Ratings)

```bash
cd mentor-sync/src/Modules/Ratings
```

3. Run EF core command:

```bash
dotnet ef migrations add AddReviewsInitial --context RatingsDbContext --project ./MentorSync.Ratings/MentorSync.Ratings.csproj --startup-project ../../MentorSync.API/MentorSync.API.csproj --output-dir Data/Migrations
```

4. Go to the **MigrationsService** project
5. Add reference to **desired** module project
6. In **DependencyInjectionExtensions** file in **AddDbContexts method** register dedicated DbContext:

```csharp
AddDbContext<RatingsDbContext>(SchemaConstants.Ratings);
```

7. In Worker file add sample code:

```csharp
await MigrateAsync<RatingsDbContext>(scope.ServiceProvider, cancellationToken);
// also you can pass additional seed method as postMigrationStep parameter
```
