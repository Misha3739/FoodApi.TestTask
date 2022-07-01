using FoodApi.TestTask.Models;

namespace FoodApi.TestTask.Helpers;

public class WordOccurrencesHelper : IWordOccurrencesHelper
{
	public (string word,int occurences) FindMaxOccurrences(RecallDateResultModel model)
	{
		Dictionary<string, int> repeatedWords = new Dictionary<string, int>();
		foreach (var item in model.Recalls.Where(r => !string.IsNullOrEmpty(r.reason_for_recall)))
		{
			var words = item.reason_for_recall.Split(" ");
			foreach (var word in words.Where(w => w.Length >= 4))
			{
				if (!repeatedWords.ContainsKey(word))
				{
					repeatedWords.Add(word, 1);
				}
				else
				{
					repeatedWords[word]++;
				}
			}
		}

		var repeatedWord = repeatedWords.MaxBy(w => w.Value);
		return (repeatedWord.Key, repeatedWord.Value);
	}
}