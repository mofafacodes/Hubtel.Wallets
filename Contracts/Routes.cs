namespace Hubtel.Wallets.Contracts
{
    public class Routes
    {
        public const string Version = "vi";

        public const string Root = "api";

        public const string Base = Root + "/" + Version;

        public static class WalletRoutes
        {
            public const string WalletBase = Base + "/" + "wallets";

            public const string ById = "{id}";

            public const string Create = WalletBase + "/" + "create";

            public const string GetAllAdmin = WalletBase + "/" + "/all";

            public const string GetAllOwner = WalletBase + "/" + "owner";

        }
    }
}
