using FoodApi.TestTask.Domain;
using Newtonsoft.Json;
using RestSharp;

namespace FoodApi.TestTask.Core;

public class RestExecutor : IRestExecutor
{
	public async Task<T?> GetAsync<T>(string url) where T : class, new()
	{
		using var restClient = new RestClient();
		var restRequest = new RestRequest(url);
		var result = await restClient.ExecuteAsync<FoodApiQueryRequestBody>(restRequest);
		if (result.IsSuccessful)
		{
			return JsonConvert.DeserializeObject<T>(result.Content);
		}

		return default(T);
	}
}