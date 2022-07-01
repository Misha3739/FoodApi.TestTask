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
		var queryString =
			$"{_baseUrl}?search=report_date:[{fromUtc.DateToQueryString()}+TO+{toUtc.DateToQueryString()}]&limit=1";
		var data = await _restExecutor.GetAsync<FoodApiQueryRequestBody>(queryString);
		if (data?.results != null && data.results.Count > 0)
		{
			var reportDateStr = data.results[0].report_date;
			return reportDateStr.ResponseStringToDate();
		}


		return null;
	}
}