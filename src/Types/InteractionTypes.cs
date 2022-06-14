using Interacord.Types.Components;

namespace Interacord.Types
{
    /// <summary>
    /// The interaction class. Used when recieving an interaction. See <see cref="Interacord.Context.InteractionContext.Data"/> for mor info.
    /// </summary>
    public class Interaction
    {
        public string Id { get; set; } = null!;
        public string ApplicationId { get; set; } = null!;
        public EInteractionType Type { get; set; }
        public InteractionData? Data { get; set; }
        public string? GuildId { get; set; }
        public string? ChannelId { get; set; }
        public GuildMember? Member { get; set; }
        public User? User { get; set; }
        public string Token { get; set; } = null!;
        public int Version { get; set; }
        public Message? Message { get; set; }
        public string? Locale { get; set; }
        public string? Guild_locale { get; set; }
    }
    /// <summary>
    /// More interaction data for <see cref="Interaction"/>
    /// </summary>
    public class InteractionData
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Type { get; set; }
        public ResolvedData? Resolved { get; set; }
        public List<InteractionOption>? Options { get; set; } = new();
        public string? CustomId { get; set; }
        public EInteractionComponentT? ComponentType { get; set; }
        public List<string>? Values { get; set; }
        public string? TargetId { get; set; }
        public List<Component>? Components { get; set; }
    }
    /// <summary>
    /// A class for command option's. Used in <seealso cref="Interacord.Types.InteractionData.Options"/>
    /// </summary>
    public class InteractionOption : IEquatable<InteractionOption>
    {
        public string Name { get; set; } = null!;
        public EInteractionOptionType Type { get; set; }
        public System.Text.Json.JsonElement? Value { get; set; }
        public List<InteractionOption>? Options { get; set; }
        public bool? Focused { get; set; }
        public bool Equals(InteractionOption? other)
        {
            if (other == null) return false;
            return (this.Name.Equals(other.Name));
        }
    }

    /// <summary>
    /// Resolved user and member's data. More resolved data is gonna get added later!
    /// </summary>
    public class ResolvedData
    {
        public Dictionary<string, User>? Users { get; set; }
        public Dictionary<string, GuildMember>? Members { get; set; }
    }

    /// <summary>
    /// A enum for interaction type.
    /// </summary>
    public enum EInteractionType
    {
        PING = 1,
        APPLICATION_COMMAND = 2,
        MESSAGE_COMPONENT = 3,
        APPLICATION_COMMAND_AUTOCOMPLETE = 4,
        MODAL_SUBMIT = 5,
    }

    /// <summary>
    /// A enum for command option.
    /// </summary>
    public enum EInteractionOptionType
    {
        SubCommand = 1,
        SubCommandGroup = 2,
        String = 3,
        Integer = 4,
        Bool = 5,
        User = 6,
        Channel = 7,
        Role = 8,
        Mentionable = 9,
        Number = 10,
        Attachment = 11
    }
}