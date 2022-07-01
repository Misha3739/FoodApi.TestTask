using FoodApi.TestTask.Models;

namespace FoodApi.TestTask.Helpers;

public interface IWordOccurrencesHelper
{
	(string word,int occurences) FindMaxOccurrences(RecallDateResultModel model);
}