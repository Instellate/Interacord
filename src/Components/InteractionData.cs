using Interacord.Types;
using Interacord.Types.Embed;
using Interacord.Types.Components;

namespace Interacord.Components
{
    /// <summary>
    /// A class that stores the initial response data.
    /// </summary>
    public class InteractionRespData
    {
        public EInteractionCallback Type { get; set; }
        public InteractionResponseData? Data { get; set; }
        public InteractionRespData(InteractionResponseData data, EInteractionCallback type = (EInteractionCallback)4)
        {
            this.Data = data;
            this.Type = type;
        }
        public InteractionRespData(EInteractionCallback type)
        {
            this.Type = type;
        }
        public InteractionRespData()
        {
            this.Type = ((EInteractionCallback)4);
            this.Data = new InteractionResponseData();
        }
    }
    /// <summary>
    /// A class that stores most of the interaction data.
    /// </summary>
    public class InteractionResponseData {
        public bool? Tts { get; set; }
        public string? Content { get; set; }
        public List<Embed> Embeds { get; set; }
        public int? Flags { get; set; }
        public List<Component>? Components { get; set; }
        public List<Attachment>? Attachments { get; set; }
        public InteractionResponseData()
        {
            this.Attachments = new List<Attachment>();
            this.Embeds = new List<Embed>();
            this.Components = new List<Component>();
        }
        public void AddComponent(Component component)
        {
            this.Components?.Add(component);
        }
    }
    /// <summary>
    /// A enum for interaction callbacks.
    /// </summary>
    public enum EInteractionCallback
    {
        PONG = 1,
        CHANNEL_MESSAGE_WITH_SOURCE = 4,
        DEFERRED_CHANNEL_MESSAGE_WITH_SOURCE = 5,
        DEFERRED_UPDATE_MESSAGE = 6,
        UPDATE_MESSAGE = 7,
        APPLICATION_COMMAND_AUTOCOMPLETE_RESULT = 8,
        MODAL = 9
    }
}
