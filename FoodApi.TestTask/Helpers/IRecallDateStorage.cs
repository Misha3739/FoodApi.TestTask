using FoodApi.TestTask.Domain;
using FoodApi.TestTask.Models;

namespace FoodApi.TestTask.Helpers;

public interface IRecallDateStorage
{
	void AddOrUpdateItem(DateTime reportDate, FoodApiQueryRequestResult result);
	
	IDictionary<DateTime, RecallDateResultModel> Storage { get; }
	
	void CleanUp();
}