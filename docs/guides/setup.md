# How to setup Interacord
Interacord is made to be easy to use Discord library. It is made to be easy to use and uses static method's for command execution. 


## Start
The library is mainly based around the [Client](/api/Interacord.Client.html) class.

The client class contains 2 important parameters. The first one for the super secret bot token. And the second one being the public key used for identifying messages. We also provide a third parameter that is to define the port. The default for that parameter is 8080. Discord requires you to use an HTTPS connection for the endpoint. So we recommend you to use a HTTP tunnel that provides you an HTTPS connection as for example ngrok.

Here's an example on how you could initialize the client class:
```cs
Client _client = new Client(Environment.GetEnvironmentVariable("botToken")!, Environment.GetEnvironmentVariable("publicKey")!);
```


## Registration process
We use two methods for a registration process.
Those are RegisterCommands and RegisterMessageComponents.

You need to provide your current app domain's assemblies.
A way you can do it is by doing 

```cs
client.RegisterCommands(AppDomain.CurrentDomain.GetAssemblies());
client.RegisterMessageComponents(AppDomain.CurrentDomain.GetAssemblies());`
```
These will register the command's and message component's for you!


# Command 

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

	await ctx.FollowUp(embed: embed);

	return;
}
```

Guide's and FAQs for this library will be increased and made better overtime!
