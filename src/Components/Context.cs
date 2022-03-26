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
        public Interaction? Data { get; set; }
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

            this._resp.Close();
        }
        /// <summary>
        /// Let's you defer the reply and reply later on. Give's you more time.
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
        public async Task FollowUp(string messageContent = null!, Embed embed = null!, ActionRow component = null!, ActionRow[] components = null!, bool ephemeral = false)
        {
            using (HttpClient webClient = new HttpClient())
            {
                Message respData = new Message();

                respData.Content = messageContent;
                if (component != null) respData.Components!.Add(component);
                if (components != null) respData.Components!.AddRange(components);
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
        /// </summary>
        /// <param name="messageContent">The message content for the reply.</param>
        /// <param name="embed">The message embed for the reply.</param>
        /// <param name="component">A message component for the reply. Only use if you intend to give one component.</param>
        /// <param name="components">Multiple message components for the reply.</param>
        /// <param name="ephemeral">Decides if the message is gonna be ephemeral or not.</param>
        public async Task EditReply(string messageContent = null!, Embed embed = null!, ActionRow component = null!, ActionRow[] components = null!, bool ephemeral = false)
        {
            using (HttpClient webClient = new HttpClient())
            {

                Message respData = new Message();

                respData.Content = messageContent;
                if (component != null) respData.Components!.Add(component);
                if (components != null) respData.Components!.AddRange(components);
                if (embed != null) respData.Embeds.Add(embed);

                if (ephemeral)
                {
                    BitField bitField = new BitField();
                    bitField.SetOn((ulong)MessageFlag.Ephemeral);

                    respData!.Flags = (int)bitField.Mask;
                }

                var data = JsonSerializer.Serialize(respData, new JsonSerializerOptions() { PropertyNamingPolicy = new SnakeCaseNamingPolicy() });

                var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");

                using (HttpResponseMessage response = await webClient.PatchAsync($"https://discord.com/api/v10/webhooks/{Data!.ApplicationId}/{Data!.Token}/messages/{Data!.Message!.Id}", content)) { }
            }
        }
    }

    /// <summary>
    /// A interaction context class for slash commands.
    /// </summary>
    /// <seealso cref="InteractionContext"/>
    public class CommandContext : InteractionContext
    {
        public CommandContext(HttpListenerResponse resp, Interaction data) : base(resp, data){}
        /// <summary>
        /// Gets a option that was provided when the interaction was executed.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>A string with the option value or null.</returns>
        public string? GetOptionsString(string name)
        {
            var result = this.Data!.Data!.Options!.Find(x => x.Name.Contains(name)) ?? null!;

            if (result == null) return null!;

            return result.Value;
        }
    }
    /// <summary>
    /// A interaction context class for message components.
    /// </summary>
    /// <seealso cref="InteractionContext"/>
    public class ComponentContext : InteractionContext
    {
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
