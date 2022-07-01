﻿using FoodApi.TestTask.Core;
using FoodApi.TestTask.Domain;
using FoodApi.TestTask.Helpers;
using FoodApi.TestTask.Models;

namespace FoodApi.TestTask.FoodRestApi;

public class FoodRestApiExecutor
{
	private readonly IRestExecutor _restExecutor;
	private readonly string _baseUrl;

	public FoodRestApiExecutor(IRestExecutor restExecutor, string baseUrl)
	{
		_restExecutor = restExecutor;
		_baseUrl = baseUrl;
	}

	public async Task<RecallDateResultModel?> FindReportDateWithFewestRecallAsync(DateTime fromUtc, DateTime toUtc)
	{
		var size = 1000;
		long counter = 0;
		long? total = null;
		Dictionary<DateTime, RecallDateResultModel> recallDates = new Dictionary<DateTime, RecallDateResultModel>();
		do
		{
			var queryString =
				$"{_baseUrl}?search=report_date:[{fromUtc.DateToQueryString()}+TO+{toUtc.DateToQueryString()}]&limit={size}&skip={counter}";
			var data = await _restExecutor.GetAsync<FoodApiQueryRequestBody>(queryString);
			total = data.meta.results.total;
			foreach (var result in data.results)
			{
				var reportDate = result.report_date.ResponseStringToDate();
				if (reportDate.HasValue)
				{
					if (!recallDates.ContainsKey(reportDate.Value))
					{
						var item = new RecallDateResultModel()
						{
							ReportDate = reportDate.Value,
							Recalls = new List<FoodApiQueryRequestResult>() { result }
						};
						recallDates.Add(reportDate.Value, item);
					}
					else
					{
						recallDates[reportDate.Value].Recalls.Add(result);
					}
				}
			}
			
			counter += data!.meta.results.total;
		} while (counter < total.Value);

		if (recallDates.Any())
		{
			var result = recallDates.MinBy(d => d.Value.Recalls.Count).Value;
			result.Recalls = result.Recalls.OrderBy(r => r.RecallInitiationDate).ToList();

			Dictionary<string, int> repeatedWords = new Dictionary<string, int>();
			foreach (var item in result.Recalls.Where(r => !string.IsNullOrEmpty(r.reason_for_recall)))
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

			result.RepeatedWord.Word = repeatedWord.Key;
			result.RepeatedWord.Occurences = repeatedWord.Value;
			return result;
		}

		return null;
	}
}