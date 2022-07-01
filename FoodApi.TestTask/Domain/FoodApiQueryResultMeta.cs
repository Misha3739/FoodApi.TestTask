namespace FoodApi.TestTask.Domain;

public class FoodApiQueryResultMeta
{
	public string disclaimer { get; set; }
	
	public string terms { get; set; }
	public FoodApiQueryResultTotals results { get; set; }
}