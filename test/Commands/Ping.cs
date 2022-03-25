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

            var option = new SelectOption() { Label = "Hello!", Value = "hello", Description = "Hello world!" };
            selectMenu.AddOption(option);

            selectMenu.MinValues = 1;
            selectMenu.MaxValues = 2;

            option = new SelectOption() { Label = "Hi!", Value = "hi", Description = "Hi world!" };
            selectMenu.AddOption(option);

            component.AddComponent(button);
            component2.AddComponent(selectMenu);

            ActionRow[] componentArray = { component, component2 };

            string? fick = ctx.GetOptionsString("string");

            ctx.Reply("Reached! " + ctx.GetOptionsString("stringy"), components: componentArray);
        }

        [CommandName(name: "ping")]
        public static async Task Ping(CommandContext ctx)
        {
            Embed embed = new Embed();

            embed.SetTitle("Pong!");
            embed.SetDescription("A response!");
            embed.SetColor(0xFFC0CB);

            await ctx.FollowUp(embed: embed);

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

            string response = $"<@{ctx.Data!.Member!.User!.Id}> selected ";

            var options = ctx.GetSelectOptions();

            int i = 0;
            foreach (var option in options)
            {
                if (i == 0)
                {
                    switch (option)
                    {
                        case "hi":
                            response = response + "Hi! ";
                            break;
                        case "hello":
                            response = response + "Hello! ";
                            break;
                    }
                }
                else
                {
                    switch (option)
                    {
                        case "hi":
                            response = response + "and Hi!";
                            break;
                        case "hello":
                            response = response + "and Hello!";
                            break;
                    }
                }
                i++;
            }

            await ctx.EditReply(response);
        }
    }
}
