1. Restore the repository-local EF Core tool:

```bash
dotnet tool restore
```

2. Ensure that all services are registered in the IoC container.
3. Go to the specific module folder (for example, Ratings).

```bash
cd mentor-sync/src/Modules/Ratings
```

4. Verify that the model has no uncommitted changes before adding a migration:

```bash
dotnet ef migrations has-pending-model-changes --context RatingsDbContext --project ./MentorSync.Ratings/MentorSync.Ratings.csproj --startup-project ../../MentorSync.API/MentorSync.API.csproj
```

5. Run the EF Core command:

```bash
dotnet ef migrations add AddReviewsInitial --context RatingsDbContext --project ./MentorSync.Ratings/MentorSync.Ratings.csproj --startup-project ../../MentorSync.API/MentorSync.API.csproj --output-dir Data/Migrations
```

6. Go to the **MigrationService** project.
7. Add a reference to the desired module project.
8. In **DependencyInjectionExtensions**, register the dedicated `DbContext` in
   `AddDbContexts`:

```csharp
AddDbContext<RatingsDbContext>(SchemaConstants.Ratings);
```

9. In `Worker`, add the migration call:

```csharp
await MigrateAsync<RatingsDbContext>(scope.ServiceProvider, cancellationToken);
// also you can pass additional seed method as postMigrationStep parameter
```

Migration files and snapshots must remain compiled in their owning module.
Do not hand-edit generated migration files.
