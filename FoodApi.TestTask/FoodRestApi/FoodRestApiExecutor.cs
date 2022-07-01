using FoodApi.TestTask.Core;
using FoodApi.TestTask.Domain;
using FoodApi.TestTask.Helpers;
using FoodApi.TestTask.Models;

namespace FoodApi.TestTask.FoodRestApi;

public class FoodRestApiExecutor
{
	private readonly IRestExecutor _restExecutor;
	private readonly IRecallDateStorage _recallDateStorage;
	private readonly IWordOccurrencesHelper occurrencesHelper;
	private readonly string _baseUrl;

	public FoodRestApiExecutor(IRestExecutor restExecutor, string baseUrl, IRecallDateStorage recallDateStorage, IWordOccurrencesHelper occurrencesHelper)
	{
		_restExecutor = restExecutor;
		_baseUrl = baseUrl;
		_recallDateStorage = recallDateStorage;
		this.occurrencesHelper = occurrencesHelper;
	}

	public async Task<RecallDateResultModel?> FindReportDateWithFewestRecallAsync(DateTime fromUtc, DateTime toUtc)
	{
		try
		{
			var size = 1000;
			long counter = 0;
			long? total = null;
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
						_recallDateStorage.AddOrUpdateItem(reportDate.Value, result);
					}
				}

				counter += data!.meta.results.total;
			} while (counter < total.Value);

			if (_recallDateStorage.Storage.Any())
			{
				var result = _recallDateStorage.Storage.MinBy(d => d.Value.Recalls.Count).Value;
				result.Recalls = result.Recalls.OrderBy(r => r.RecallInitiationDate).ToList();

				var repeatedWord = occurrencesHelper.FindMaxOccurrences(result);

				result.RepeatedWord.Word = repeatedWord.word;
				result.RepeatedWord.Occurences = repeatedWord.occurences;
				return result;
			}

			return null;
		}
		finally
		{
			_recallDateStorage.CleanUp();
		}
	}
}