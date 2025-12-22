namespace Entities.ConfigurationModels
{
    public class JwtConfiguration
    {
        public const string Section = "JwtSettings"; // nom de la section
        public string Secret { get; set; } = null!;
        public string ValidIssuer { get; set; } = null!;
        public string ValidAudience { get; set; } = null!;
        public int Expires { get; set; }
    }

}
