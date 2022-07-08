using Interacord.Types;
using Interacord.Types.Embed;
using Interacord.Types.Components;
using Interacord.Components;
using System.Net;
using System.Text.Json;
using BitFields;
using System.Net.Http.Json;

namespace Interacord.Context
{
    /// <summary>
    /// A base class for Interactions.
    /// </summary>
    /// <remarks>Do not use this in your parameters. This do not give you all of the needed data.</remarks>
    public abstract class InteractionContext
    {
        /// <summary>
        /// The raw interaction data provided from discord
        /// </summary>
        public Interaction? Data { get; set; }
        /// <summary>
        /// The Http Response. Used for initial usage methods.
        /// </summary>
        internal HttpListenerResponse _resp { get; set; }
        internal InteractionContext(HttpListenerResponse resp, Interaction data)
        {
            this.Data = data;
            this._resp = resp;
        }
        /// <summary>
        /// A method for replying to a interaction.
        /// </summary>
        /// <param name="messageContent">The message content for the reply.</param>
        /// <param name="embed">The message embed for the reply.</param>
        /// <param name="component">A message component for the reply. Only use if you intend to give one component.</param>
        /// <param name="components">Multiple message components for the reply.</param>
        /// <param name="ephemeral">Decides if the reply is gonna be ephemeral or not.</param>
        public void Reply(string messageContent = null!, Embed embed = null!, ActionRow component = null!, ActionRow[] components = null!, bool ephemeral = false)
        {
            InteractionRespData respData = new InteractionRespData();

            if (embed != null) respData.Data!.Embeds.Add(embed);
            if (component != null) respData.Data!.Components!.Add(component);
            if (components != null) respData.Data!.Components!.AddRange(components);
            respData.Data!.Content = messageContent;

            if (ephemeral)
            {
                BitField bitField = new BitField();
                bitField.SetOn((ulong)MessageFlag.Ephemeral);

                respData.Data!.Flags = (int)bitField.Mask;
            }

            string respDataString = JsonSerializer.Serialize(respData, new JsonSerializerOptions() { PropertyNamingPolicy = new SnakeCaseNamingPolicy() });
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(respDataString);

            this._resp.ContentLength64 = buffer.Length;

            this._resp.OutputStream.Write(buffer, 0, buffer.Length);
            this._resp.OutputStream.Flush();
            this._resp.OutputStream.Dispose();

            this._resp.Close();
        }
        /// <summary>
        /// Lets you defer the reply and reply later on. Give's you more time.
        /// </summary>
        /// <param name="ephemeral">Decides if the loading state is gonna be ephemeral or not.</param>
        public void DeferReply(bool ephemeral = false)
        {
            InteractionRespData respData = new InteractionRespData();

            if (ephemeral)
            {
                BitField bitField = new BitField();
                bitField.SetOn((ulong)MessageFlag.Ephemeral);

                respData.Data!.Flags = (int)bitField.Mask;
            }

            if (this.Data!.Data!.ComponentType != null) respData.Type = EInteractionCallback.DEFERRED_UPDATE_MESSAGE;
            else respData.Type = ((EInteractionCallback)5);

            string respDataString = JsonSerializer.Serialize(respData, new JsonSerializerOptions() { PropertyNamingPolicy = new SnakeCaseNamingPolicy() });
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(respDataString);

            this._resp.ContentLength64 = buffer.Length;

            this._resp.OutputStream.Write(buffer, 0, buffer.Length);
            this._resp.OutputStream.Flush();
            this._resp.OutputStream.Dispose();

            this._resp.Close();
        }
        
        /// <summary>
        /// Make a follow up to a reply.
        /// </summary>
        /// <param name="messageContent">The message content for the follow up.</param>
        /// <param name="embed">The message embed for the follow up.</param>
        /// <param name="component">A message component for the follow up. Only use if you intend to give one component.</param>
        /// <param name="components">Multiple message components for the follow up.</param>
        /// <param name="ephemeral">Decides if the message is gonna be ephemeral or not.</param>
        /// <returns></returns>
        public async Task FollowUpAsync(string messageContent = null!, Embed embed = null!, ActionRow component = null!, ActionRow[] components = null!, bool ephemeral = false)
        {
            using (HttpClient webClient = new HttpClient())
            {
                Message respData = new Message();

                respData.Content = messageContent;
                if (component != null) respData.Components!.Add(component);
                else respData.Components = null;
                if (components != null) respData.Components!.AddRange(components);
                else respData.Components = null;
                if (embed != null) respData.Embeds.Add(embed);

                if (ephemeral)
                {
                    BitField bitField = new BitField();
                    bitField.SetOn((ulong)MessageFlag.Ephemeral);

                    respData!.Flags = (int)bitField.Mask;
                }

                var data = JsonSerializer.Serialize(respData, new JsonSerializerOptions() { PropertyNamingPolicy = new SnakeCaseNamingPolicy() });
                using (HttpResponseMessage response = await webClient.PostAsJsonAsync($"https://discord.com/api/v10/webhooks/{Data!.ApplicationId}/{Data!.Token}", respData, new JsonSerializerOptions() { PropertyNamingPolicy = new SnakeCaseNamingPolicy() })) { }
            }
        }
        /// <summary>
        /// Edit the reply of the current interaction.
        /// Use <see cref="EditReply" /> if you want to edit without a initial usage.
        /// </summary>
        /// <param name="messageContent">The message content for the reply.</param>
        /// <param name="embed">The message embed for the reply.</param>
        /// <param name="component">A message component for the reply. Only use if you intend to give one component.</param>
        /// <param name="components">Multiple message components for the reply.</param>
        /// <param name="ephemeral">Decides if the message is gonna be ephemeral or not.</param>
        public async Task EditReplyAsync(string messageContent = null!, Embed embed = null!, ActionRow component = null!, ActionRow[] components = null!, bool ephemeral = false)
        {
            using (HttpClient webClient = new HttpClient())
            {

                Message respData = new Message();

                respData.Content = messageContent;
                if (component != null) respData.Components!.Add(component);
                else respData.Components = null;
                if (components != null) respData.Components!.AddRange(components);
                else respData.Components = null;
                respData.Content = messageContent;
                if (embed != null) respData.Embeds.Add(embed);

                if (ephemeral)
                {
                    BitField bitField = new BitField();
                    bitField.SetOn((ulong)MessageFlag.Ephemeral);

                    respData!.Flags = (int)bitField.Mask;
                }

                var data = JsonSerializer.Serialize(respData, new JsonSerializerOptions() { PropertyNamingPolicy = new SnakeCaseNamingPolicy() });

                var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");

                if (this.Data!.Data!.ComponentType != null) using (HttpResponseMessage response = await webClient.PatchAsync($"https://discord.com/api/v10/webhooks/{Data!.ApplicationId}/{Data!.Token}/messages/{Data!.Message!.Id}", content)) { }
                else using (HttpResponseMessage response = await webClient.PatchAsync($"https://discord.com/api/v10/webhooks/{Data!.ApplicationId}/{Data!.Token}/messages/@original", content)) { };
            }
        }

        /// <summary>
        /// Edit the reply of the current interaction.
        /// Use <see cref="EditReplyAsync" /> if you want to edit after doing a initial usage.
        /// </summary>
        /// <param name="messageContent">The message content for the reply.</param>
        /// <param name="embed">The message embed for the reply.</param>
        /// <param name="component">A message component for the reply. Only use if you intend to give one component.</param>
        /// <param name="components">Multiple message components for the reply.</param>
        /// <param name="ephemeral">Decides if the message is gonna be ephemeral or not.</param>
        public void EditReply(string messageContent = null!, Embed embed = null!, ActionRow component = null!, ActionRow[] components = null!, bool ephemeral = false)
        {
            InteractionRespData respData = new InteractionRespData();

            if (embed is not null) respData.Data!.Embeds.Add(embed);

            if (component is not null) respData.Data!.Components!.Add(component);
            else if(components is not null) respData.Data!.Components!.AddRange(components);
            else respData.Data!.Components = null;
            
            respData.Data!.Content = messageContent;

            if (ephemeral)
            {
                BitField bitField = new BitField();
                bitField.SetOn((ulong)MessageFlag.Ephemeral);

                respData.Data!.Flags = (int)bitField.Mask;
            }

            respData.Type = EInteractionCallback.UPDATE_MESSAGE;

            string respDataString = JsonSerializer.Serialize(respData, new JsonSerializerOptions() { PropertyNamingPolicy = new SnakeCaseNamingPolicy() });
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(respDataString);

            this._resp.ContentLength64 = buffer.Length;

            this._resp.OutputStream.Write(buffer, 0, buffer.Length);
            this._resp.OutputStream.Flush();
            this._resp.OutputStream.Dispose();

            this._resp.Close();
        }
    }

    /// <summary>
    /// A interaction context class for slash commands.
    /// </summary>
    /// <seealso cref="InteractionContext"/>
    public class CommandContext : InteractionContext
    {
        public CommandContext(HttpListenerResponse resp, Interaction data) : base(resp, data) { }
        /// <summary>
        /// Gets a string option that was provided when the interaction was executed.
        /// </summary>
        /// <param name="name">The option string name.</param>
        /// <returns>The option value as a string or null.</returns>
        /// <exception cref="InvalidOptionTypeException" />
        public string? GetOptionString(string name)
        {
            var result = this.Data!.Data!.Options!.Find(x => x.Name.Equals(name)) ?? null!;

            if (result is null) return null!;
            if (result.Type != EInteractionOptionType.String) throw new InvalidOptionTypeException($"You can not use type {result.Type.ToString()} as a string.");

            return Convert.ToString(result.Value);
        }

        /// <summary>
        /// Gets a integer option that was provided when the interaction was executed.
        /// </summary>
        /// <param name="name">The option string name.</param>
        /// <returns>The option value as a integer or null.</returns>
        /// <exception cref="InvalidOptionTypeException" />
        public int? GetOptionInt(string name)
        {
            var result = this.Data!.Data!.Options!.Find(x => x.Name.Equals(name)) ?? null!;

            if (result is null) return null!;
            if (result.Type != EInteractionOptionType.Integer) throw new InvalidOptionTypeException($"You can not use type {result.Type.ToString()} as a integer.");

            return result.Value.GetValueOrDefault().GetInt32();
        }

        /// <summary>
        /// Gets a double option that was provided when the interaction was executed.
        /// </summary>
        /// <param name="name">The option string name.</param>
        /// <returns>The option value as a double or null.</returns>
        /// <exception cref="InvalidOptionTypeException" />
        public double? GetOptionDouble(string name)
        {
            var result = this.Data!.Data!.Options!.Find(x => x.Name.Equals(name)) ?? null!;

            if (result.Type != EInteractionOptionType.Integer) throw new InvalidOptionTypeException($"You can not use type {result.Type.ToString()} as a double.");
            if (result is null) return null!;

            return result.Value.GetValueOrDefault().GetDouble();
        }

        /// <summary>
        /// Returns the original message after initial usage method has been used.
        /// </summary>
        /// <returns></returns>
        public async Task<Message>? GetOriginalMessage()
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync($"https://discord.com/api/v10/webhooks/{Data!.ApplicationId}/{Data.Token}/messages/@original"))
                {
                    var rawData = await response.Content.ReadAsStringAsync();
                    var Data = JsonSerializer.Deserialize<Message?>(rawData);

                    return Data ?? null!;
                }
            }
        }
    }

    public class InvalidOptionTypeException : Exception
    {
        public InvalidOptionTypeException(string message) : base(message)
        {

        }
    }

    /// <summary>
    /// A interaction context class for message components.
    /// </summary>
    /// <seealso cref="InteractionContext"/>
    public class ComponentContext : InteractionContext
    {
        public List<string>? ComponentParameters { get; internal set; }

        public ComponentContext(HttpListenerResponse resp, Interaction data) : base(resp, data) { }
        /// <summary>
        /// Gets a string list with an array of selected object's in a select menu.
        /// </summary>
        /// <returns>A list of strings.</returns>
        public List<string> GetSelectOptions()
        {
            return Data!.Data!.Values!;
        }
    }
}
