using Interacord.Components.Commands;
using Interacord.Types.Embed;
using Interacord.Types.Components;
using Interacord.Context;

namespace test.Commands
{
    public class Utility
    {

        [CommandName(name: "test")]
        public static void Test(CommandContext ctx)
        {
            var component = new ActionRow();
            var component2 = new ActionRow();

            var button = new Button() { Style = EButtonStyle.Primary, CustomId = "button", Label = "Hello!" };
            var selectMenu = new SelectMenu() { CustomId = "selectMenu" };

            selectMenu.AddOption("Hello!", "hello");

            selectMenu.MinValues = 1;
            selectMenu.MaxValues = 2;

            selectMenu.AddOption("Hi!", "hi");

            component.AddComponent(button);
            component2.AddComponent(selectMenu);

            ActionRow[] componentArray = { component, component2 };

            string? fick = ctx.GetOptionsString("string");

            ctx.Reply("Reached! " + ctx.GetOptionsString("stringy"), components: componentArray);
        }

        [CommandName(name: "ping")]
        public static void Ping(CommandContext ctx)
        {
            Embed embed = new Embed();

            embed.SetTitle("Pong!");
            embed.SetDescription("A response!");
            embed.SetColor(0xFFC0CB);

            ctx.Reply(embed: embed);

            return;
        }

        [MessageComponent(id: "button", type: "Button")]
        public static async Task TestButton(ComponentContext ctx)
        {
            string response = "<@" + ctx.Data!.Member!.User!.Id + "> clicked the button!";

            ctx.DeferReply(ephemeral: true);

            await ctx.EditReply(response);

            return;
        }

        [MessageComponent(id: "selectMenu", type: "SelectMenu")]
        public static async Task TestSelectMenu(ComponentContext ctx)
        {
            ctx.DeferReply(ephemeral: true);

            var options = ctx.GetSelectOptions();

            string selected = string.Join(", ", options.ToArray());

            string response = $"<@{ctx.Data!.Member!.User!.Id}> selected {selected}";

            await ctx.EditReply(response);
        }
    }
}
