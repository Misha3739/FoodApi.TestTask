namespace FoodApi.TestTask.Domain;

public class FoodApiQueryRequestBody
{
	public FoodApiQueryResultMeta meta { get; set; }
	public List<FoodApiQueryRequestResult> results { get; set; }
}