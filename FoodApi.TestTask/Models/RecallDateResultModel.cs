using FoodApi.TestTask.Domain;

namespace FoodApi.TestTask.Models;

public class RecallDateResultModel
{
	public DateTime? ReportDate { get; set; }
	
	public MostlyRepeatedWord RepeatedWord { get; }
	public List<FoodApiQueryRequestResult> Recalls { get; set; }

	public RecallDateResultModel()
	{
		Recalls = new List<FoodApiQueryRequestResult>();
		RepeatedWord = new MostlyRepeatedWord();
	}
}