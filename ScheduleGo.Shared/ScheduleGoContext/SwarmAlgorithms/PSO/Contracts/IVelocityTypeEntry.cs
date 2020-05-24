namespace ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Contracts
{
    public interface IVelocityTypeEntry
    {
        IVelocityTypeEntry Initialize(double[] minValues, double[] maxValues);
        IVelocityTypeEntry Clone();
        void Update(double weight,
                    double particleWeight,
                    double swarmWeight,
                    IPositionTypeEntry position,
                    IPositionTypeEntry bestPosition,
                    IPositionTypeEntry swarmBestPosition);
    }
}