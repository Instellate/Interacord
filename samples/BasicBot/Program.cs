using Interacord;
using Interacord.Components;

public class Program
{
    public static void Main()
    {
        Client _client = new Client(Environment.GetEnvironmentVariable("botToken")!, Environment.GetEnvironmentVariable("publicKey")!);
        _client.RegisterCommands(AppDomain.CurrentDomain.GetAssemblies());
        _client.RegisterMessageComponents(AppDomain.CurrentDomain.GetAssemblies());
        _client.Start();

        Thread.Sleep(-1);
    }
}