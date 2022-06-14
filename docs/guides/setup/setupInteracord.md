---
uid: Setup.InteracordSetup
Title: How to setup Interacord.
---

# How to setup Interacord
Interacord is made to be easy to use Discord library. It uses static method's for command execution. 

> [!NOTE]
> If you don't know how to setup interaction endpoints. Please read **[Setup interaction endpoint](/guides/setup/initialSetup.html)**


## Start
The main and required point to start the bot is through the [Client](/api/Interacord.Client.html) class.

The client class contains 2 required parameters. The first one for the secret bot token. And the second one being the public key used for identifying messages. We also provide a third parameter that is to define the port. The default for that parameter is 8080.

Here's an example on how you could initialize the client class:
```cs
Client _client = new Client(config["SecretKey"]!, config["PublicKey"]!);
```
Of course is this just

## Registration process
We use two methods for a registration process.
Those are RegisterCommands and RegisterMessageComponents.

You need to provide your current app domain's assemblies.
A way you can do it is by doing 

```cs
client.RegisterCommands(AppDomain.CurrentDomain.GetAssemblies());
client.RegisterMessageComponents(AppDomain.CurrentDomain.GetAssemblies());
```
These will register the command's and message component's for you!


# Commands

How is the command structure?

It's really simple!
Here's a simple Ping command with a embed for example
```cs
[CommandName(name: "ping")]
public static async Task Ping(CommandContext ctx)
{
	Embed embed = new Embed();

	embed.SetTitle("Pong!");
	embed.SetDescription("A response!");
	embed.SetColor(0xFFC0CB);

	await ctx.Reply(embed: embed);

	return;
}
```

If you have any more questions. Please make a issue as no official server exist yet.