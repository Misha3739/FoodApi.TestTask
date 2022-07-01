﻿// See https://aka.ms/new-console-template for more information

using FoodApi.TestTask.Core;
using FoodApi.TestTask.FoodRestApi;
using Newtonsoft.Json;

Console.WriteLine("Food api test task");

try
{
	var baseUrl = "https://api.fda.gov/food/enforcement.json";
	var restExecutor = new RestExecutor();
	var executor = new FoodRestApiExecutor(restExecutor, baseUrl);
	var dateTimeFromUtc = new DateTime(2012, 1, 1);
	var dateTimeToUtc = new DateTime(2012, 12, 31);
	var fewestRecallDateResult = await executor.FindReportDateWithFewestRecallAsync(dateTimeFromUtc,dateTimeToUtc);
	if (fewestRecallDateResult != null && fewestRecallDateResult.ReportDate.HasValue)
	{
		Console.WriteLine($"Fewest recall date is : {fewestRecallDateResult.ReportDate.Value:yyyy-MM-dd}");
		foreach (var item in fewestRecallDateResult.Recalls)
		{
			Console.WriteLine($"\"{item.RecallInitiationDate}\": \"{JsonConvert.SerializeObject(item)}\"");
		}
	}
	else
	{
		Console.WriteLine($"OOPS, we didn't find any report date for the selected period");
	}
}
catch (Exception e)
{
	Console.WriteLine($"OOPS, smth was going wrong... {e}");
}

