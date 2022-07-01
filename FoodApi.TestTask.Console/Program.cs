// See https://aka.ms/new-console-template for more information

using FoodApi.TestTask.Core;
using FoodApi.TestTask.FoodRestApi;

Console.WriteLine("Hello, World!");

var baseUrl = "https://api.fda.gov/food/enforcement.json";
var restExecutor = new RestExecutor();
var executor = new FoodRestApiExecutor(restExecutor, baseUrl);
var dateTimeFromUtc = new DateTime(2012, 1, 1);
var dateTimeToUtc = new DateTime(2012, 12, 31);
await executor.FindReportDateWithFewestRecallAsync(dateTimeFromUtc,dateTimeToUtc);