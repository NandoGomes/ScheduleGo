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
		}
		public virtual TLeftProperty Left { get; private set; }
		public virtual TRightProperty Right { get; private set; }

		public static implicit operator TLeftProperty(LinkEntity<TLeftProperty, TRightProperty> entity) => entity.Left;
		public static implicit operator TRightProperty(LinkEntity<TLeftProperty, TRightProperty> entity) => entity.Right;

		public bool Equals(IEntity other) => (other?.GetType().IsAssignableFrom(this.GetType()) ?? false) && this.Left?.Id == (other as LinkEntity<TLeftProperty, TRightProperty>)?.Left?.Id && this.Right?.Id == (other as LinkEntity<TLeftProperty, TRightProperty>)?.Right?.Id;
	}
}