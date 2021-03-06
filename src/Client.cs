using Interacord.Components;
using Interacord.Components.Commands;
using Interacord.Types;
using Interacord.Context;
using NSec.Cryptography;
using System.Net;
using System.Reflection;
using System.Text.Json;

namespace Interacord
{
    /// <summary>
    /// The client class.
    /// </summary>
    public class Client
    {
        private string _botToken;
        private string _publicKey;
        private int _port;
        internal HttpListener _webServer = new();
        internal PublicKey _importedKey;
        internal Dictionary<string, CommandObject> _commands = new();
        internal Dictionary<string, MessageComponentObject> _messageComponents = new();
        /// <summary>
        /// The primary constructor
        /// </summary>
        /// <param name="botToken">The bot token</param>
        /// <param name="publicKey">The public key</param>
        /// <param name="port">The port. Primarily 8080</param>
        public Client(string botToken, string publicKey, int port = 8080)
        {
            this._botToken = botToken;
            this._publicKey = publicKey;
            this._port = port;
            this._webServer.Prefixes.Add("http://127.0.0.1:" + this._port + '/');

            var algorithm = SignatureAlgorithm.Ed25519;
            this._importedKey = PublicKey.Import(algorithm, Convert.FromHexString(this._publicKey), KeyBlobFormat.RawPublicKey);
        }

        /// <summary>
        /// A method used to start the bot.
        /// </summary>
        public void Start()
        {
            _webServer.Start();
            _webServer.BeginGetContext(RequestManagement, this);
        }

        /// <summary>
        /// A method for slash command registration
        /// </summary>
        /// <param name="assembly">A assembly array for finding the method's and registrating them</param>
        public void RegisterCommands(Assembly[] assembly)
        {
            this._commands = CommandService.AddCommands(assembly);
            return;
        }

        /// <summary>
        /// A method for message components.
        /// </summary>
        /// <param name="assembly">A assembly array for finding the method's and registrating them</param>
        public void RegisterMessageComponents(Assembly[] assembly)
        {
            this._messageComponents = CommandService.AddMessageComponets(assembly);
            return;
        }

        /// <summary>
        /// A method that handles request's.
        /// </summary>
        /// <param name="result"></param>
        private static async void RequestManagement(IAsyncResult result)
        {
            var algorithm = SignatureAlgorithm.Ed25519;

            var _client = result.AsyncState as Client;
            var _webServer = _client!._webServer;

            var ctx = _webServer.EndGetContext(result);
            _webServer.BeginGetContext(RequestManagement, _client);

            var req = ctx.Request;
            var resp = ctx.Response;

            if (req.HttpMethod != "POST") { resp.Close(); return; }

            bool isVerifiedReq;

            System.Text.Encoding encoding = req.ContentEncoding;
            string body;

            using (Stream encodedBody = req.InputStream)
            using (System.IO.StreamReader reader = new(encodedBody, encoding))
            {
                body = await reader.ReadToEndAsync();

                string reqSign = req.Headers.Get("X-Signature-Ed25519")!;
                string reqData = req.Headers.Get("X-Signature-Timestamp")! + body;

                byte[] reqByteData = System.Text.Encoding.Default.GetBytes(reqData);
                var reqHexSign = Convert.FromHexString(reqSign);

                isVerifiedReq = algorithm.Verify(_client._importedKey, reqByteData, reqHexSign);
            }

            if (!isVerifiedReq) { resp.Close(); return; };

            var serializerOptions = new JsonSerializerOptions() { PropertyNamingPolicy = new SnakeCaseNamingPolicy() };
            Interaction? interactionData = JsonSerializer.Deserialize<Interaction>(body, serializerOptions);

            if (interactionData == null) { resp.Close(); return; }

            resp.AddHeader("Content-Type", "application/json");

            if (interactionData.Type == EInteractionType.PING)
            {
                InteractionRespData interactionResp = new((EInteractionCallback)1);
                string respDataString = JsonSerializer.Serialize(interactionResp, serializerOptions);
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(respDataString);

                resp.ContentLength64 = buffer.Length;

                resp.OutputStream.Write(buffer, 0, buffer.Length);

                resp.Close();

            } else if (interactionData.Type == EInteractionType.APPLICATION_COMMAND) {
                CommandObject? command = new CommandObject();

                _client._commands.TryGetValue(interactionData.Data!.Name, out command);

                if (command == null) return;

                var interactionContext = new CommandContext(resp, interactionData);

                object[] methodArgs = { interactionContext };

                command.Method!.Invoke(null, methodArgs);

            } else
            {
                MessageComponentObject? component = new();

                _client._messageComponents.TryGetValue(interactionData.Data!.CustomId!, out component);

                if (component == null) return;

                if (component.Type != interactionData.Data.ComponentType) return;

                var interactionContext = new ComponentContext(resp, interactionData);

                object[] methodArgs = { interactionContext };

                component.Method!.Invoke(null, methodArgs);
            }

            if (resp != null) resp.Close();
            return;
        }
    }

    /// <summary>
    /// A class for sname case naming policy.
    /// </summary>
    internal class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public static SnakeCaseNamingPolicy Instance { get; } = new SnakeCaseNamingPolicy();

        public override string ConvertName(string name)
        {
            return JsonPolicies.ToSnakeCase(name);
        }
    }
}

internal static class JsonPolicies
{
    public static string ToSnakeCase(string str)
    {
        return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
    }
}