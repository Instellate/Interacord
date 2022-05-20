namespace Interacord.Types.Embed
{
    /// <summary>
    /// A class for embeds
    /// </summary>
    public class Embed
    {
        public string? Title { get; set; }
        public string? Type { get; set; } = "rich";
        public string? Description { get; set; }
        public string? Url { get; set; }
        public string? Timestamp { get; set; }
        public int? Color { get; set; }
        public EmbedFooter? Footer { get; set; }
        public EmbedImage? Image { get; set; }
        public EmbedThumbnail? Thumbnail { get; set; }
        public EmbedVideo? Video { get; set; }
        public EmbedProvider? Provider { get; set; }
        public EmbedAuthor? Author { get; set; }
        public List<EmbedField> Fields { get; set; }

        public Embed()
        {
            this.Fields ??= new List<EmbedField>();
        }
        /// <summary>
        /// Set's the title of the embed.
        /// </summary>
        /// <param name="value">The value of the title.</param>
        public Embed SetTitle(string value)
        {
            this.Title = value;
            return this;
        }
        /// <summary>
        /// Set's the description of the embed.
        /// </summary>
        /// <param name="value">The value of the description.</param>
        public Embed SetDescription(string value)
        {
            this.Description = value;
            return this;
        }
        /// <summary>
        /// Add's a field to the embed.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <param name="value">The value of the field.</param>
        public Embed AddField(string name, string value)
        {
            var field = new EmbedField();

            field.Name = name;
            field.Value = value;

            this.Fields.Add(field);
            return this;
        }
        /// <summary>
        /// Set's the color of the embed.
        /// </summary>
        /// <param name="hex">The hex of the color.</param>
        public Embed SetColor(uint hex)
        {
            this.Color = (int)hex;
            return this;
        }
    }
    /// <summary>
    /// The embed field class.
    /// </summary>
    /// <seealso cref="Embed"/>
    public class EmbedField
    {
        public string Name { get; set; } = null!;
        public string Value { get; set; } = null!;
        public bool? Inline { get; set; } = null!;
    }
    /// <summary>
    /// The embed author class.
    /// </summary>
    /// <seealso cref="Embed"/>
    public class EmbedAuthor
    {
        public string Name { get; set; } = null!;
        public Uri? Url { get; set; }
        public Uri? IconUrl { get; set; }
        public Uri? ProxyIconUrl { get; set; }
    }
    /// <summary>
    /// The embed provider class.
    /// </summary>
    /// <seealso cref="Embed"/>
    public class EmbedProvider
    {
        public string? Name { get; set; }
        public Uri? Url { get; set; }
    }
    /// <summary>
    /// The embed video class.
    /// </summary>
    /// <seealso cref="Embed"/>
    public class EmbedVideo
    {
        public Uri? Url { get; set; }
        public Uri? ProxyUrl { get; set; }
        public int? Height { get; set; }
        public int? Width { get; set; }
    }
    /// <summary>
    /// The embed thumbnail class.
    /// </summary>
    /// <seealso cref="Embed"/>
    public class EmbedThumbnail
    {
        public string Url { get; set; } = null!;
        public Uri? ProxyUrl { get; set; } = null!;
        public int? Height { get; set; }
        public int? Width { get; set; }
    }
    /// <summary>
    /// The embed image class.
    /// </summary>
    /// <seealso cref="Embed"/>
    public class EmbedImage
    {
        public string Url { get; set; } = null!;
        public Uri? ProxyUrl { get; set; }
        public int? Height { get; set; }
        public int? Width { get; set; }
    }
    /// <summary>
    /// The embed footer
    /// </summary>
    /// <seealso cref="Embed"/>
    public class EmbedFooter
    {
        public string Text { get; set; } = null!;
        public Uri? IconUrl { get; set; }
        public string? ProxyIconUrl { get; set; }
    }
}