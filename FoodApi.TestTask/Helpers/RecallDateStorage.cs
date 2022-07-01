using FoodApi.TestTask.Domain;
using FoodApi.TestTask.Models;

namespace FoodApi.TestTask.Helpers;

public class RecallDateStorage : IRecallDateStorage
{
	private readonly Dictionary<DateTime, RecallDateResultModel> _storage;

	public RecallDateStorage()
	{
		_storage = new Dictionary<DateTime, RecallDateResultModel>();
	}
	
	public void AddOrUpdateItem(DateTime reportDate, FoodApiQueryRequestResult result)
	{
		if (!_storage.ContainsKey(reportDate))
		{
			var item = new RecallDateResultModel()
			{
				ReportDate = reportDate,
				Recalls = new List<FoodApiQueryRequestResult>() { result }
			};
			_storage.Add(reportDate, item);
		}
		else
		{
			_storage[reportDate].Recalls.Add(result);
		}
	}

	public void CleanUp()
	{
		_storage.Clear();
	}

	public IDictionary<DateTime, RecallDateResultModel> Storage => _storage;
}