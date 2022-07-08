
namespace Interacord.Registration;

public abstract class SlashCommandRegistration
{
    public abstract string Name { get; set; }
    public abstract string Description { get; set; }
    public Types.EInteractionType Type { get; set; } = Types.EInteractionType.APPLICATION_COMMAND;
    public bool? DmPermission { get; set; }
    public abstract OptionRegistration[]? Options { get; set; }
}

public class OptionRegistration
{
    public Types.EInteractionOptionType Type { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool Required { get; set; } = false; 
}