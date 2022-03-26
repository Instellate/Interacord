using System.Text.Json.Serialization;
using Interacord.Types.Converter;

namespace Interacord.Types.Components
{
    /// <summary>
    /// A abstract class for message components.
    /// </summary>
    [JsonConverter(typeof(MessageComponentConverter))]
    public abstract class Component
    {
        public EInteractionComponentT Type { get; set; }
    }
    /// <summary>
    /// A class for action rows.
    /// </summary>
    public class ActionRow : Component
    {
        public List<Component>? Components { get; set; } = new();
        public ActionRow()
        {
            this.Type = EInteractionComponentT.ActionRow;
        }
        /// <summary>
        /// A shortcut to add a component.
        /// </summary>
        /// <param name="component">The component to add.</param>
        public void AddComponent(Component component)
        {
            this.Components?.Add(component);
        }
        public void AddComponents(Component[] components)
        {
            this.Components?.AddRange(components);
        }
    }
    /// <summary>
    /// A class for text input
    /// </summary>
    public class TextInput : Component
    {
        public string CustomId { get; set; } = null!;
        public ETextInputStyle Style { get; set; }
        public string Label { get; set; } = null!;
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
        public bool? Required { get; set; }
        public string? Value { get; set; }
        public string? Placeholder { get; set; }
        public TextInput()
        {
            this.Type = EInteractionComponentT.TextInput;
        }
    }
    /// <summary>
    /// A class for select menus.
    /// </summary>
    public class SelectMenu : Component
    {
        public string CustomId { get; set; } = null!;
        public List<SelectOption> Options { get; set; }
        public string? Placeholder { get; set; }
        public int? MinValues { get; set; }
        public int? MaxValues { get; set; }
        public bool? Disabled { get; set; }
        public SelectMenu()
        {
            this.Options = new List<SelectOption>();
            this.Type = EInteractionComponentT.SelectMenu;
        }
        public void AddOption(string label, string value, bool defaultOption = false)
        {
            SelectOption option = new() { Value = value, Label = label, Default = defaultOption };

            Options.Add(option);
        }
    }
    /// <summary>
    /// A class for buttons.
    /// </summary>
    public class Button : Component
    {
        public EButtonStyle Style { get; set; }
        public string? Label { get; set; }
        public EmojiClass? Emoji { get; set; }
        public string? CustomId { get; set; }
        public Uri? Url { get; set; }
        public bool? Disabled { get; set; }
        public Button()
        {
            this.Type = EInteractionComponentT.Button;
        }
    }
    /// <summary>
    /// A class for select options used for <see cref="SelectMenu"/>
    /// </summary>
    public class SelectOption
    {
        public string Label { get; set; } = null!;
        public string Value { get; set; } = null!;
        public string? Description { get; set; }
        public EmojiClass? Emoji { get; set; }
        public bool? Default { get; set; }
    }
    /// <summary>
    /// A enum for interaction component types.
    /// </summary>
    public enum EInteractionComponentT
    {
        ActionRow = 1,
        Button = 2,
        SelectMenu = 3,
        TextInput = 4,
    }
    /// <summary>
    /// A enum for button styles.
    /// </summary>
    public enum EButtonStyle
    {
        Primary = 1,
        Secondary = 2,
        Sucess = 3,
        Danger = 4,
        Link = 5
    }
    /// <summary>
    /// Style for text input.
    /// </summary>
    public enum ETextInputStyle
    {
        Short = 1,
        Paragraph = 2,
    }
}