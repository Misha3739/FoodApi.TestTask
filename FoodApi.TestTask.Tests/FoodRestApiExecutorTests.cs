﻿using System.Reflection;
using FoodApi.TestTask.Core;
using FoodApi.TestTask.Domain;
using FoodApi.TestTask.FoodRestApi;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace FoodApi.TestTask.Tests;

[TestFixture]
public class FoodRestApiExecutorTests
{
	private readonly string _baseUrl = "https://api.fda.gov/food/enforcement.json";
	private readonly Mock<IRestExecutor> _restExecutorMock;
	private readonly FoodRestApiExecutor _executor;

	public FoodRestApiExecutorTests()
	{
		_restExecutorMock = new Mock<IRestExecutor>();
		_executor = new FoodRestApiExecutor(_restExecutorMock.Object,_baseUrl);
	}
	
	[Test]
	public async Task FindReportDateWithFewestRecallAsync_DateFound_ReturnsExpectedDate()
	{
		var path = Path.Combine(Environment.CurrentDirectory, "Files", "TestData.json");
		var fileContent = await File.ReadAllTextAsync(path);
		var response = JsonConvert.DeserializeObject<FoodApiQueryRequestBody>(fileContent);

		_restExecutorMock.Setup(e => e.GetAsync<FoodApiQueryRequestBody>(It.IsAny<string>()))
			.ReturnsAsync(response);

		var dateTimeFrom = new DateTime(2012, 1, 1);
		var dateTimeTo = new DateTime(2012, 12, 31);
		var actual = await _executor.FindReportDateWithFewestRecallAsync(dateTimeFrom, dateTimeTo);

		string expectedUrl = $"{_baseUrl}?search=report_date:[20120101+TO+20121231]&limit=1";
		_restExecutorMock.Verify(e => e.GetAsync<FoodApiQueryRequestBody>(expectedUrl), Times.Exactly(1));

		Assert.IsNotNull(actual);
		var expectedDate = new DateTime(2012,8,15);
		Assert.AreEqual(expectedDate, actual);
	}
}