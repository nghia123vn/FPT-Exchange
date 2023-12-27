namespace Service.OAuth.Model
{
    public class GoogleAuthSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public string RedirectUrl { get; } = "https://localhost:44336/api/google-service";
    }
}
