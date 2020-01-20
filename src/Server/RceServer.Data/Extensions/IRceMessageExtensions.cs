using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RceServer.Domain.Models.Messages;

namespace RceServer.Data.Extensions
{
	public static class IRceMessageExtensions
	{
		public static ExpandoObject ToExpandoObject(this IRceMessage rceMessage)
		{
			var data = JsonConvert.SerializeObject(rceMessage);
			return JsonConvert.DeserializeObject<ExpandoObject>(data, new ExpandoObjectConverter());
		}
	}
}
