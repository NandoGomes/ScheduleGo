namespace ScheduleGo.Domain.ScheduleGoContext.SwarmAlgorithms.PSO.Enums
{
	public enum EValidationCosts : int
	{
		GravePenalty = 1000,
		MediumPenalty = 100,
		SmallPenalty = 10,
		SmallBonus = -10,
		MediumBonus = -100,
		BigBonus = -1000
	}
}