using System.Net.Http.Json;
using FoodApi.TestTask.Core;
using FoodApi.TestTask.Domain;
using FoodApi.TestTask.Helpers;
using Newtonsoft.Json;
using RestSharp;

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

	public async Task<DateTime?> FindReportDateWithFewestRecallAsync(DateTime fromUtc, DateTime toUtc)
	{
		var size = 1000;
		long counter = 0;
		long? total = null;
		Dictionary<DateTime, int> recallDates = new Dictionary<DateTime, int>();
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
						recallDates.Add(reportDate.Value, 1);
					}
					else
					{
						recallDates[reportDate.Value]++;
					}
				}
			}
			
			counter += data!.meta.results.total;
		} while (counter < total.Value);

		if (recallDates.Any())
		{
			return recallDates.OrderBy(d => d.Value).First().Key;
		}

		return null;
	}
}