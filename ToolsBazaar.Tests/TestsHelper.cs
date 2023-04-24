namespace ToolsBazaar.Tests
{
    internal static class TestsHelper
    {
        private static readonly Random _random = new();

        internal static int GetRandomInt()
        {
            return _random.Next(1, 100);
        }

        internal static string GetRandomString(int length)
        {
            // Max length of Guid is 22 chars
            var str = Guid.NewGuid().ToString("n").Substring(0, length);
            return str;
        }

        internal static string GetRandomEmail()
        {
            var rndString = GetRandomString(10);
            var email = $"{rndString.Substring(0, 4)}@{rndString.Substring(4, 6)}.com";
            return email;
        }
    }
}
