using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.DomainEvents;
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

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.HasDefaultSchema(SchemaConstants.Users);

		ConfigureIdentityTables(builder);

		// Configure one-to-one relationship between MenteeProfile and AppUser
		builder.Entity<MenteeProfile>()
			.HasOne(mp => mp.User)
			.WithOne()
			.HasForeignKey<MenteeProfile>(mp => mp.MenteeId)
			.OnDelete(DeleteBehavior.Cascade);

		// Configure one-to-one relationship between MentorProfile and AppUser
		builder.Entity<MentorProfile>()
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

	private static void ConfigureIdentityTables(ModelBuilder builder)
	{
		// reconfigure base Identity tables
		builder.Entity<AppUser>(b =>
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

		builder.Entity<AppRole>(b =>
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

		builder.Entity<AppUser>().ToTable("Users");
		builder.Entity<AppRole>().ToTable("Roles");
		builder.Entity<AppUserToken>().ToTable("UserTokens");
		builder.Entity<AppUserRole>().ToTable("UserRoles");
		builder.Entity<AppRoleClaim>().ToTable("RoleClaims");
		builder.Entity<AppUserClaim>().ToTable("UserClaims");
		builder.Entity<AppUserLogin>().ToTable("UserLogins");
	}
}
