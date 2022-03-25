using System.Reflection;
using Interacord.Types.Components;

namespace Interacord.Components.Commands
{
    internal class CommandService
    {
        internal static Dictionary<string, CommandObject> AddCommands(Assembly[] assembly)
        {
            var Result = new Dictionary<string, CommandObject>();
            var methods = assembly
                .SelectMany(s => s.GetTypes())
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(true).OfType<CommandNameAttribute>().Any());

            foreach (var method in methods)
            {
                if (method.DeclaringType!.IsNotPublic || !method.IsStatic) { continue; }

                CommandObject commandData = new CommandObject(method);
                var nameData = method.GetCustomAttribute(typeof(CommandNameAttribute)) as CommandNameAttribute;
                Result.Add(nameData!.CommandName, commandData);
            }

            return Result;
        }
        internal static Dictionary<string, MessageComponentObject> AddMessageComponets(Assembly[] assembly)
        {
            var Result = new Dictionary<string, MessageComponentObject>();
            var methods = assembly
                .SelectMany(s => s.GetTypes())
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(true).OfType<MessageComponentAttribute>().Any());

            foreach (var method in methods)
            {
                if (method.DeclaringType!.IsNotPublic || !method.IsStatic) { continue; }

                var nameData = method.GetCustomAttribute(typeof(MessageComponentAttribute)) as MessageComponentAttribute;
                MessageComponentObject componentData = new MessageComponentObject(method, nameData!.Type);

                Result.Add(nameData!.Id, componentData);
            }

            return Result;
        }
    }
    /// <summary>
    /// A object used to store MethodInfo for commands and slash commands.
    /// </summary>
    public class CommandObject
    {
        public MethodInfo? Method { get; set; }
        public Dictionary<string, CommandObject>? SubCommands { get; set; }
        public CommandObject(Dictionary<string, CommandObject> subCommands)
        {
            this.SubCommands = subCommands;
        }
        public CommandObject(MethodInfo method)
        {
            this.Method = method;
        }
        public CommandObject() { }
    }
    /// <summary>
    /// A object used to store MethodInfo for message components.
    /// </summary>
    public class MessageComponentObject
    {
        public MethodInfo Method { get; set; } = null!;
        public EInteractionComponentT Type { get; set; }

        public MessageComponentObject(MethodInfo method, EInteractionComponentT type)
        {
            this.Method = method;
            this.Type = type;
        }
        internal MessageComponentObject(){}
    }
    /// <summary>
    /// A attribute used to define a command to a method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandNameAttribute : Attribute
    {
        private string _commandName;
        private string? _subCommandName;

        public CommandNameAttribute(string name)
        {
            this._commandName = name;
        }

        public CommandNameAttribute(string name, string subName)
        {
            this._commandName = name;
            this._subCommandName = subName;
        }
        public virtual string CommandName { get { return _commandName; } }
        public virtual string? SubCommandName { get { return _subCommandName; } }
    }
    /// <summary>
    /// A attrbute used to define a component to a method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class MessageComponentAttribute : Attribute
    {
        private string _componentId;
        private EInteractionComponentT _componentType;

        public MessageComponentAttribute(string id, string type)
        {
            this._componentId = id;
            Enum.TryParse(type, out this._componentType);

        }

        public virtual string Id { get { return _componentId; } }
        public virtual EInteractionComponentT Type { get { return _componentType; } }
    }
}