using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MentorSync.SharedKernel.BaseEntities;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Domain;

public sealed class AppUser : IdentityUser<int>, IHaveDomainEvents
{
    private readonly List<DomainEvent> _domainEvents = [];

    [StringLength(300)]
    public string RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    [StringLength(500)]
    public string ProfileImageUrl { get; set; }
    public bool IsActive { get; set; }
    [StringLength(2000)]
    public string Bio { get; set; }
    [Length(1, 5)]
    public List<string> CommunicationLanguages { get; set; }
    [StringLength(30)]
    public string Country { get; set; }
    [NotMapped]
    public IEnumerable<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public ICollection<AppUserClaim> Claims { get; set; }
    public ICollection<AppUserLogin> Logins { get; set; }
    public ICollection<AppUserToken> Tokens { get; set; }
    public ICollection<AppUserRole> UserRoles { get; set; }

    public void ClearDomainEvents() => _domainEvents.Clear();

    public void RaiseDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
