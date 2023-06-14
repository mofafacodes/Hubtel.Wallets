namespace Hubtel.Wallets.Contracts
{
    public class Routes
    {
        public const string Version = "v1";

        public const string Root = "api";

        public const string Base = Root + "/" + Version;

        public static class WalletRoutes
        {

            public const string Create = Base + "/wallets" + "/create";

            public const string GetAllAdmin = Base + "/wallets" + "/all";

            public const string GetAllOwner = Base + "/wallets" + "/owner";

            public const string ById = Base + "/wallets" + "/{id}";

        }
    }
}
