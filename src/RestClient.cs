using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Interacord.Types;
using Interacord.Types.Embed;
using System.Collections.Generic;
using System.Text.Json;

namespace Interacord
{
    public class RestClient
    {
        private string _token;
        public string Token { get { return _token; } private set { _token = value; } }

        public RestClient(string token)
        {
            _token = token;
            return;
        }

        public async Task Send(string channelId, string? content)
        {
            using (HttpClient client = new HttpClient())
            await using (var stream = System.IO.File.OpenRead(@"C:\Users\love_\OneDrive\Bilder\AnimeFanart\87033410_p0.png"))
            {
                using var multiContent = new MultipartFormDataContent();

                multiContent.Add(new StreamContent(stream), "files[0]", "anime.png");

                Message respData = new Message();

                Embed embed = new();

                embed.SetTitle("Hello world!");
                embed.SetDescription("How are you?");

                embed.Image = new EmbedImage { Url = "attachment://anime.png" };

                respData.Embeds.Add(embed);

                var data = JsonSerializer.Serialize(respData, new JsonSerializerOptions() { PropertyNamingPolicy = new SnakeCaseNamingPolicy() });

                multiContent.Add(new StringContent(data, System.Text.Encoding.UTF8, "application/json"), "payload_json");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bot", this._token);
                client.BaseAddress = new Uri("discord.com/api/v10/");

                var resp = await client.PostAsync("channels/775293747205242920/messages", multiContent);
            }
            return;
        }
    }
}
