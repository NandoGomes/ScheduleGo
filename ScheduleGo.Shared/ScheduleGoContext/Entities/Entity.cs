using System;

namespace ScheduleGo.Shared.ScheduleGoContext.Entities
{
	public abstract class Entity : IEquatable<Entity>
	{
		public Entity() => Id = Guid.NewGuid();

		public Guid Id { get; private set; }

		public bool Equals(Entity other) => this.Id == other?.Id;
	}
}