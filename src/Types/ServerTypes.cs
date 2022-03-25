namespace Interacord.Types
{
    public class Role
    {
        public string Name { get; set; } = null!;
        public string Permissions { get; set; } = null!;
        public int Color { get; set; }
        public bool Hoist { get; set; }
    }
    public class ChannelMention
    {
        public string Id { get; set; } = null!;
        public string GuildId { get; set; } = null!;
        public int Type { get; set; }
        public string Name { get; set; } = null!;
    }
    public class Channel
    {
        public string Id { get; set; } = null!;
        public int Type { get; set; }
        public string? GuildId { get; set; }
        public int? Position { get; set; }
        public Overwrite[]? PermissionOverwrites { get; set; }
        public string? Name { get; set; }
        public string? Topic { get; set; }
        public bool? Nsfw { get; set; }
        public string? LastMessageId { get; set; }
        public int? Bitrate{ get; set; }
        public int? UserLimit { get; set; }
        public int? RateLimitPerUser { get; set; }
        public User[]? Recipients { get; set; }
        public Uri? Icon { get; set; }
        public string? OwnerId { get; set; }
        public string? ApplicationId { get; set; }
        public string? ParentId { get; set; }
        public string? LastPinTimestamp { get; set; }
        public string? RtcRegion { get; set; }
        public int? VideoQualityMode { get; set; }
        public int? MessageCount { get; set; }
        public int? MemberCount { get; set; }
        public ThreadMetdata? ThreadMetadata { get; set; }
        public ThreadMember? Member { get; set; }
        public int? DefaultAutoArchiveDuration { get; set; }
        public string? Permissions { get; set; }
    }

    public class ThreadMember
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string JoinTimestamp { get; set; } = null!;
        public int Flags { get; set; }
    }

    public class ThreadMetdata
    {
        public bool Archived { get; set; }
        public int AutoArchiveDuration { get; set; }
        public string ArchiveTimestamp { get; set; } = null!;
        public bool Locked { get; set; }
        public bool? Invitable { get; set; }
        public string? CreateTimestamp { get; set; }
    }

    public class Overwrite
    {
        public string Id { get; set; } = null!;
        public int Type { get; set; }
        public string Allow { get; set; } = null!;
        public string Disallow { get; set; } = null!;
    }
}