using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Interacord.Types;
using System.Text.Json.Nodes;
using Interacord.Types.Components;

namespace Interacord.Types.Converter
{
    internal class MessageComponentConverter : JsonConverter<Component>
    {
        public override Component Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var messageComponent = JsonSerializer.Deserialize<JsonObject>(ref reader);

            if (messageComponent!.ContainsKey("type") && messageComponent["type"] is JsonValue val && val.TryGetValue<int>(out int v))
            {
                return v switch
                {
                    1 => JsonSerializer.Deserialize<ActionRow>(messageComponent, options)!,
                    2 => JsonSerializer.Deserialize<Button>(messageComponent, options)!,
                    3 => JsonSerializer.Deserialize<SelectMenu>(messageComponent, options)!,
                    4 => JsonSerializer.Deserialize<TextInput>(messageComponent, options)!,
                    _ => throw new InvalidMessageComponentType("The type number is invalid and cannot be deserialized")
                };
            } else
            {
                return null!;
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Component value, JsonSerializerOptions options)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(value, value?.GetType() ?? null!, options);
            using var token = JsonDocument.Parse(bytes);
            token.RootElement.WriteTo(writer);
        }
    }
    [Serializable]
    internal class InvalidMessageComponentType : Exception
    {
        public InvalidMessageComponentType() : base() { }
        public InvalidMessageComponentType(string message) : base(message) { }
        public InvalidMessageComponentType(string message, Exception inner) : base(message, inner) { }
        protected InvalidMessageComponentType(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}