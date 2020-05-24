using System;

namespace ScheduleGo.Shared.ScheduleGoContext.Entities
{
    public abstract class Entity : IEntity
    {
        public Entity() => Id = Guid.NewGuid();

        public Guid Id { get; private set; }

        public bool Equals(IEntity other) => (other?.GetType().IsAssignableFrom(this.GetType()) ?? false) && this.Id == (other as Entity)?.Id;
    }
}