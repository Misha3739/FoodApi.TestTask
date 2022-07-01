using FoodApi.TestTask.Helpers;
using NUnit.Framework;

namespace FoodApi.TestTask.Tests;
[TestFixture]
public class FoodApiQueryHelperTests
{
	[TestCase(2012, 1, 1, "20120101")]
	[TestCase(2012, 12, 31,"20121231")]
	public void ToFoodApiQueryString_DateToString_ReturnsExpectedString(int year, int month, int day, string expected)
	{
		var dateTime = new DateTime(year, month, day);
		
		var actual = dateTime.DateToQueryString();
		
		Assert.AreEqual(expected, actual);
	}
	
	[TestCaseSource(nameof(ValidStringDateTestCases))]
	public void ToFoodApiDateTime_ValidStringToDate_ReturnsExpectedDate(string dateString, DateTime expected)
	{
		var actual = dateString.ResponseStringToDate();
		
		Assert.IsNotNull(actual);
		Assert.AreEqual(expected, actual);
	}
	
	[TestCase("2012")]
	[TestCase("201201")]
	[TestCase("")]
	public void ToFoodApiDateTime_InValidStringToDate_ReturnsNull(string dateString)
	{
		var actual = dateString.ResponseStringToDate();
		
		Assert.IsNull(actual);
	}

	private static IEnumerable<TestCaseData> ValidStringDateTestCases()
	{
		yield return new TestCaseData("20120101", new DateTime(2012, 1, 1));
		yield return new TestCaseData("20121231", new DateTime(2012, 12, 31));
	}
}