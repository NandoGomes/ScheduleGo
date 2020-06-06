using System;

namespace ScheduleGo.Shared.ScheduleGoContext.Entities
{
	public class LinkEntity<TLeftProperty, TRightProperty> : IEntity where TLeftProperty : Entity where TRightProperty : Entity
	{
		protected LinkEntity() { }
		public LinkEntity(TLeftProperty left, TRightProperty right)
		{
			Left = left;
			Right = right;

			LeftId = left.Id;
			RightId = right.Id;
		}

		public Guid LeftId { get; private set; }
		public Guid RightId { get; private set; }

		public virtual TLeftProperty Left { get; private set; }
		public virtual TRightProperty Right { get; private set; }

		public static implicit operator TLeftProperty(LinkEntity<TLeftProperty, TRightProperty> entity) => entity.Left;
		public static implicit operator TRightProperty(LinkEntity<TLeftProperty, TRightProperty> entity) => entity.Right;

		public bool Equals(IEntity other) => (other?.GetType().IsAssignableFrom(this.GetType()) ?? false) && this.LeftId == (other as LinkEntity<TLeftProperty, TRightProperty>)?.LeftId && this.RightId == (other as LinkEntity<TLeftProperty, TRightProperty>)?.RightId;
	}
}