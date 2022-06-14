namespace Interacord.Types
{
    public class User
    {
        public string Id { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Discriminator { get; set; } = null!;
        public Uri? Avatar { get; set; }
        public bool? Bot { get; set; }
        public bool? System { get; set; }
        public bool? MfaEnabled { get; set; }
        public string? Banner { get; set; }
        public int? AccentColor { get; set; }
        public string? Locale { get; set; }
        public bool? Verified { get; set; }
        public string? Email { get; set; }
        public int? Flags { get; set; }
        public int? PremiumType { get; set; }
        public int? PublicFlags { get; set; }
    }
    public class GuildMember
    {
        public User? User { get; set; }
        public string? Nick { get; set; }
        public Uri? Avatar { get; set; }
        public string[]? Roles { get; set; }
    }
}
