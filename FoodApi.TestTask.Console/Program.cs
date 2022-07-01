// See https://aka.ms/new-console-template for more information

using FoodApi.TestTask.Core;
using FoodApi.TestTask.FoodRestApi;

Console.WriteLine("Food api test task");

try
{
	var baseUrl = "https://api.fda.gov/food/enforcement.json";
	var restExecutor = new RestExecutor();
	var executor = new FoodRestApiExecutor(restExecutor, baseUrl);
	var dateTimeFromUtc = new DateTime(2012, 1, 1);
	var dateTimeToUtc = new DateTime(2012, 12, 31);
	var fewestRecallDate = await executor.FindReportDateWithFewestRecallAsync(dateTimeFromUtc,dateTimeToUtc);
	if (fewestRecallDate.HasValue)
	{
		Console.WriteLine($"Fewest recall date is : {fewestRecallDate.Value:yyyy-MM-dd}");
	}
	else
	{
		Console.WriteLine("OOPS, fewest recall date can't be extracted...");
	}
	
}
catch (Exception e)
{
	Console.WriteLine($"OOPS, smth was going wrong... {e.ToString()}");
}

