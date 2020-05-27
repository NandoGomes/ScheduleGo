using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleGo.Shared.ScheduleGoContext.Entities;

namespace ScheduleGo.Infra.ScheduleGoContext.Configurations.Links
{
	public class LinkEntityConfiguration<TLinkEntity, TLeftEntity, TRightEntity> : IEntityTypeConfiguration<LinkEntity<TLeftEntity, TRightEntity>> where TLinkEntity : LinkEntity<TLeftEntity, TRightEntity> where TLeftEntity : Entity where TRightEntity : Entity
	{
		public void Configure(EntityTypeBuilder<LinkEntity<TLeftEntity, TRightEntity>> builder)
		{
			builder.ToTable(typeof(TLinkEntity).Name);

			builder.HasKey(link => link.Id);

			builder.Property(link => link.LeftId).HasColumnName(typeof(TLeftEntity).Name).IsRequired();
			builder.Property(link => link.RightId).HasColumnName(typeof(TRightEntity).Name).IsRequired();

			builder.HasOne(link => link.Right)
				.WithOne()
				.OnDelete(DeleteBehavior.NoAction)
				.IsRequired();
		}
	}
}