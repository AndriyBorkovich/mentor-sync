using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.DomainEvents;
using MentorSync.SharedKernel.Abstractions.Messaging;
using MentorSync.Users.Domain.Mentee;
using MentorSync.Users.Domain.Mentor;
using MentorSync.Users.Domain.Role;
using MentorSync.Users.Domain.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Users.Data;

public sealed class UsersDbContext(IDomainEventsDispatcher dispatcher, DbContextOptions<UsersDbContext> options) :
    IdentityDbContext<
        AppUser, AppRole, int,
        AppUserClaim, AppUserRole, AppUserLogin,
        AppRoleClaim, AppUserToken>(options)
{
    public DbSet<MentorProfile> MentorProfiles { get; set; }
    public DbSet<MenteeProfile> MenteeProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(SchemaConstants.Users);

        // reconfigure base Identity tables
        modelBuilder.Entity<AppUser>(b =>
        {
            // Each User can have many UserClaims
            b.HasMany(e => e.Claims)
                .WithOne(e => e.User)
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();

            // Each User can have many UserLogins
            b.HasMany(e => e.Logins)
                .WithOne(e => e.User)
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();

            // Each User can have many UserTokens
            b.HasMany(e => e.Tokens)
                .WithOne(e => e.User)
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();

            // Each User can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });

        modelBuilder.Entity<AppRole>(b =>
        {
            // Each Role can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            // Each Role can have many associated RoleClaims
            b.HasMany(e => e.RoleClaims)
                .WithOne(e => e.Role)
                .HasForeignKey(rc => rc.RoleId)
                .IsRequired();
        });

        modelBuilder.Entity<AppUser>().ToTable("Users");
        modelBuilder.Entity<AppRole>().ToTable("Roles");
        modelBuilder.Entity<AppUserToken>().ToTable("UserTokens");
        modelBuilder.Entity<AppUserRole>().ToTable("UserRoles");
        modelBuilder.Entity<AppRoleClaim>().ToTable("RoleClaims");
        modelBuilder.Entity<AppUserClaim>().ToTable("UserClaims");
        modelBuilder.Entity<AppUserLogin>().ToTable("UserLogins");

        // Configure one-to-one relationship between MenteeProfile and AppUser
        modelBuilder.Entity<MenteeProfile>()
            .HasOne(mp => mp.User)
            .WithOne()
            .HasForeignKey<MenteeProfile>(mp => mp.MenteeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure one-to-one relationship between MentorProfile and AppUser
        modelBuilder.Entity<MentorProfile>()
            .HasOne(mp => mp.User)
            .WithOne()
            .HasForeignKey<MentorProfile>(mp => mp.MentorId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        // dispatch events only if save was successful
        var entitiesWithEvents = ChangeTracker.Entries<IHaveDomainEvents>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToArray();

        await dispatcher.DispatchAsync(entitiesWithEvents);

        return result;
    }
}
