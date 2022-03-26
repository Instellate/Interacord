using Interacord.Types.Components;

namespace Interacord.Types
{
    /// <summary>
    /// A class for message. Primarily used for webhooks.
    /// </summary>
    public class Message
    {
        public string Id { get; set; } = null!;
        public string ChannelId { get; set; } = null!;
        public string? GuildId { get; set; }
        public User Author { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Timestamp { get; set; } = null!;
        public string EditedTimestamp { get; set; } = null!;
        public bool Tts { get; set; }
        public bool MentionEveryone { get; set; }
        public User[] Mentions { get; set; } = null!;
        public string[] MentionRoles { get; set; } = null!;
        public ChannelMention[]? MentionChannels { get; set; }
        public Attachment[] Attachments { get; set; } = null!;
        public List<Embed.Embed> Embeds { get; set; } = null!;
        public Reaction[]? Reactions { get; set; }
        public string? Nonce { get; set; }
        public bool Pinned { get; set; }
        public string? WebhookId { get; set; }
        public int Type { get; set; }
        public string? ApplicationId { get; set; }
        public MessageReference? MessageReference { get; set; }
        public int? Flags { get; set; }
        public Message? ReferencedMessage { get; set; }
        public MessageInteraction? Interaction { get; set; }
        public Channel? Thread { get; set; }
        public List<Component>? Components { get; set; }
        public StickerItem[]? StickerItems { get; set; }
        public Sticker? Stickers { get; set; }
        public Message()
        {
            this.Embeds = new List<Embed.Embed>();
        }
    }
    /// <summary>
    /// A class for stickers.
    /// </summary>
    public class Sticker
    {
        public string Id { get; set; } = null!;
        public string? PackId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Tags { get; set; } = null!;
        public string Asset { get; set; } = null!;
        public int Type { get; set; }
        public int FormatType { get; set; }
        public bool? Available { get; set; }
        public string? GuildId { get; set; }
        public User? User { get; set; }
        public int? SortValue { get; set; }
    }
    /// <summary>
    /// A class for sticker items.
    /// </summary>
    public class StickerItem
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int FormatType { get; set; }
    }


    public class MessageInteraction
    {
        public string Id { get; set; } = null!;
        public int Type { get; set; }
        public string Name { get; set; } = null!;
        public User User { get; set; } = null!;
        public GuildMember? Member { get; set; }
    }

    public class MessageReference
    {
        public string? MessageId { get; set; }
        public string? ChannelId { get; set; }
        public string? GuildId { get; set; }
        public bool? FailIfNotExists { get; set; }
    }
    public class Reaction
    {
        public int Count { get; set; }
        public bool Me { get; set; }
        public EmojiClass Emoji { get; set; } = null!;
    }

    public class EmojiClass
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string[]? Roles { get; set; }
        public User? User { get; set; }
        public bool? RequireColons { get; set; }
        public bool? Managed { get; set; }
        public bool? Animated { get; set; }
        public bool? Available { get; set; }
    }

    public class Attachment
    {
        public string Id { get; set; } = null!;
        public string Filename { get; set; } = null!;
        public string? Description { get; set; }
        public string? ContentType { get; set; }
        public int Size { get; set; }
        public Uri Url { get; set; } = null!;
        public Uri ProxyUrl { get; set; } = null!;
        public int? Height { get; set; }
        public int? Width { get; set; }
        public bool? Ephemeral { get; set; }
    }
}