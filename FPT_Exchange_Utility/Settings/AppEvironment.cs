namespace FPT_Exchange_Utility.Settings
{
    public static class AppEvironment
    {

        public static readonly HttpClient httpClient = new HttpClient();

        //------------------DOMAIN-----------------------

        public static readonly string USER_API_DOMAIN = "http://ocelot.users:3002";

        public static readonly string WALLET_API_DOMAIN = "http://ocelot.wallets:3003";

        public static readonly string NOTIFICATION_API_DOMAIN = "http://service.notify:3005";

        public static readonly string GATEWAY_API_DOMAIN = "http://ocelot.gateway:3000";

        //------------------PATH-----------------------

        public static readonly string USER_API_PATH_GET_ONE = "/api/users";

        public static readonly string NOTIFICATION_API_PATH_SEND = "/api/notifications";

        public static readonly string GATEWAY_API_PATH_GOOGLE_AUTH = "/api/users/google-service";

        public static readonly string WALLET_API_PATH_CREATE = "/api/wallet/kz9ijalm6067ACxopm";

        public static readonly string WALLET_API_PATH_UPDATE = "/api/wallet/os19clk52homlachjak";
    }
}
