using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MentorSync.SharedKernel.Abstractions.DomainEvents;
using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Domain.User;

/// <summary>
/// Application user entity extending IdentityUser with integer primary key.
/// </summary>
public sealed class AppUser : IdentityUser<int>, IHaveDomainEvents
{
	private readonly List<DomainEvent> _domainEvents = [];

	/// <summary>
	/// Refresh token for maintaining user sessions.
	/// </summary>
	[StringLength(300)]
	public string RefreshToken { get; set; }
	/// <summary>
	/// Expiry time of the refresh token.
	/// </summary>
	public DateTime? RefreshTokenExpiryTime { get; set; }
	/// <summary>
	/// URL of the user's profile image.
	/// </summary>
	[StringLength(500)]
	public string ProfileImageUrl { get; set; }
	/// <summary>
	/// Indicates whether the user account is active.
	/// </summary>
	public bool IsActive { get; set; }
	/// <summary>
	/// User's country of residence.
	/// </summary>
	[StringLength(100)]
	public string Country { get; set; }
	/// <summary>
	/// Domain events associated with the user entity.
	/// </summary>
	[NotMapped]
	public IEnumerable<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

	/// <summary>
	/// Navigation property for user claims.
	/// </summary>
	public ICollection<AppUserClaim> Claims { get; set; }
	/// <summary>
	/// Navigation property for user logins.
	/// </summary>
	public ICollection<AppUserLogin> Logins { get; set; }
	/// <summary>
	/// Navigation property for user tokens.
	/// </summary>
	public ICollection<AppUserToken> Tokens { get; set; }
	/// <summary>
	/// Navigation property for user roles.
	/// </summary>
	public ICollection<AppUserRole> UserRoles { get; set; }

	/// <inheritdoc />
	public void ClearDomainEvents() => _domainEvents.Clear();

	/// <inheritdoc />
	public void RaiseDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
