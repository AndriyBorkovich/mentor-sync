namespace MentorSync.Users.Contracts.Models;

public sealed class UserBasicInfoModel
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string ProfileImageUrl { get; set; }
}