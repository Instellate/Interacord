using Interacord;
using Interacord.Components;
using BitFields;

public class Program
{
    public static void Main()
    {
        Client _client = new Client(Environment.GetEnvironmentVariable("botToken")!, Environment.GetEnvironmentVariable("publicKey")!);
        _client.RegisterCommands(AppDomain.CurrentDomain.GetAssemblies());
        _client.RegisterMessageComponents(AppDomain.CurrentDomain.GetAssemblies());
        _client.Start();

        // _client.restClient?.Send(Environment.GetEnvironmentVariable("botToken")!);

        Thread.Sleep(-1);

        return;
    }
}