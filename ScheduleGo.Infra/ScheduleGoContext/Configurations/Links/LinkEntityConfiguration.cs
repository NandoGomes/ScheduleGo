using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleGo.Shared.ScheduleGoContext.Entities;

namespace ScheduleGo.Infra.ScheduleGoContext.Configurations.Links
{
	public class LinkEntityConfiguration<TLinkEntity, TLeftEntity, TRightEntity> : IEntityTypeConfiguration<TLinkEntity> where TLinkEntity : LinkEntity<TLeftEntity, TRightEntity> where TLeftEntity : Entity where TRightEntity : Entity
	{
		public void Configure(EntityTypeBuilder<TLinkEntity> builder)
		{
			builder.ToTable(typeof(TLinkEntity).Name);

			builder.HasKey(link => new { link.LeftId, link.RightId });

			builder.Property(link => link.LeftId).HasColumnName(typeof(TLeftEntity).Name).IsRequired();
			builder.Property(link => link.RightId).HasColumnName(typeof(TRightEntity).Name).IsRequired();

			builder.HasOne(link => link.Left)
				.WithMany()
				.HasForeignKey(link => link.LeftId)
				.HasPrincipalKey(left => left.Id)
				.OnDelete(DeleteBehavior.NoAction)
				.IsRequired();

			builder.HasOne(link => link.Right)
				.WithMany()
				.HasForeignKey(link => link.RightId)
				.HasPrincipalKey(right => right.Id)
				.OnDelete(DeleteBehavior.NoAction)
				.IsRequired();
		}
	}
}