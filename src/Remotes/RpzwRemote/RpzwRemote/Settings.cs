namespace RpzwRemote
{
	public static class Settings
	{
		public static string LoginUrl = "https://rceserver.azurewebsites.net/api/auth/login";
		public static string GetMessagesUrl = "https://rceserver.azurewebsites.net/api/server/messages";
		public static string RunJobUrl(string workerId) => $"https://rceserver.azurewebsites.net/api/server/workers/{workerId}/runjob";
	}
}
