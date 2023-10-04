namespace blogpessoal.Security
{
    public class Setting
    {
        private static string secret = "d128f4e56a2ff6c7e21447885a91c47cce8dd1c4f15a293231e5c6b874cc4fc1";

        public static string Secret { get => secret; set => secret = value; }
    }
}
