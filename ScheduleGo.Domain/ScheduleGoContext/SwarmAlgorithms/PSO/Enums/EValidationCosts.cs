namespace ScheduleGo.Domain.ScheduleGoContext.SwarmAlgorithms.PSO.Enums
{
    public enum EValidationCosts : int
    {
        GravePenalty = 50,
        MediumPenalty = 25,
        SmallPenalty = 5,
        SmallBonus = -5,
        MediumBonus = -25,
        BigBonus = -50
    }
}