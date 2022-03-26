using System;
using System.Collections.Generic;
using Interacord.Context;
using Interacord.Types.Components;
using Interacord.Components.Commands;

namespace BasicBot.commands
{
    public class ExampleCommand
    {
        [CommandName("example")]
        public static void Message(CommandContext ctx)
        {
            ActionRow buttonRow = new();
            ActionRow selectRow = new();

            SelectMenu selectMenu = new();

            selectMenu.CustomId = "selectMenu";
            selectMenu.MaxValues = 3;
            selectMenu.AddOption("Option 1", "1");
            selectMenu.AddOption("Option 2", "2");
            selectMenu.AddOption("Option 3", "3");

            Button button1 = new() { CustomId = "button1", Label = "Blurple button", Style = EButtonStyle.Primary };
            Button button2 = new() { CustomId = "button2", Label = "Green button with emoji", Emoji = new Interacord.Types.EmojiClass() { Name = "👍" }, Style = EButtonStyle.Sucess };
            Button button3 = new() { Label = "Gray button with link", Style = EButtonStyle.Link, Url = new Uri("https://interacord.instellate.xyz") };

            selectRow.AddComponent(selectMenu);
            Component[] buttonComponents = { button1, button2, button3 };
            buttonRow.AddComponents(buttonComponents);
            ActionRow[] components = { selectRow, buttonRow };

            ctx.Reply("You executed a command!!!", components: components);
        }

        [MessageComponent("selectMenu", "SelectMenu")]
        public static async Task SelectMenu(ComponentContext ctx)
        {
            var options = ctx.GetSelectOptions();

            string response = $"<@{ctx.Data!.Member!.User!.Id}> selected ";

            ctx.DeferReply();

            await ctx.EditReply(response + string.Join(", ", options.ToArray()));

            return;
        }

        [MessageComponent("button1", "Button")]
        public static async Task Button1(ComponentContext ctx)
        {
            ctx.DeferReply();

            await ctx.EditReply($"<@{ctx.Data!.Member!.User!.Id}> clutched the blurple button click.");

        }
        [MessageComponent("button2", "Button")]
        public static async Task Button2(ComponentContext ctx)
        {
            ctx.DeferReply();

            await ctx.EditReply($"<@{ctx.Data!.Member!.User!.Id}> clutched the green button click.");
        }
    }
}