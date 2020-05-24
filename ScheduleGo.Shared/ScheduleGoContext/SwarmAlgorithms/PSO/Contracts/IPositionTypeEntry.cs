using ScheduleGo.Shared.ScheduleGoContext.Contracts;

namespace ScheduleGo.Shared.ScheduleGoContext.SwarmAlgorithms.PSO.Contracts
{
    public interface IPositionTypeEntry : ICalculatable<IPositionTypeEntry>
    {
        IPositionTypeEntry Initialize(params object[] args);
        void Update(IVelocityTypeEntry velocity);
        IPositionTypeEntry Clone();
    }
}