using System.Globalization;

namespace FoodApi.TestTask.Helpers;

public static class FoodApiQueryHelper
{
	public static string DateToQueryString(this DateTime dateTime)
	{
		return dateTime.ToString("yyyyMMdd");
	}
	
	public static DateTime? ResponseStringToDate(this string dateTimeStr)
	{
		try
		{
			return DateTime.ParseExact(dateTimeStr, "yyyyMMdd", CultureInfo.InvariantCulture);
		}
		catch (FormatException e)
		{
			return null;
		}
	}
}