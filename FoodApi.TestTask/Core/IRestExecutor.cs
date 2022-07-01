namespace FoodApi.TestTask.Core;

public interface IRestExecutor
{
	Task<T?> GetAsync<T>(string url) where T : class, new();
}