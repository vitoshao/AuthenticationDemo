namespace Demo2.AppCode
{
    public class AppSettings
    {
        public string MagicCode { get; set; } = "";

        public JwtSettings JwtSettings { get; set; } = null!;

    }

    public class JwtSettings
    {
        public string Issuer { get; set; } = null!;
        public string SignKey { get; set; } = null!;
    }


}
