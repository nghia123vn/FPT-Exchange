namespace FPT_Exchange_Utility.Settings
{
    public class AppSetting
    {
        public string Secret { get; set; } = null!;
        public string Fee { get; set; } = null!;
        public string RedisConnectionString { get; set; } = null!;

        //send mail
        public string SendGridApiKey { get; set; } = null!;
        public string EMailAddress { get; set; } = null!;
        public string NameApp { get; set; } = null!;
    }
}
