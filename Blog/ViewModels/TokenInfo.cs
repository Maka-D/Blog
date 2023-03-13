namespace Blog.ViewModels
{
    public class TokenInfo
    {
        public string Provider { get; set; } = "JwtBearer";
        public TokenNameType TokenName { get; set; }
        public string TokenValue { get; set; }
        public DateTime? TokenExpirationTime { get; set; }
    }

    public enum TokenNameType
    {
        Access,
        Refresh
    }
}
