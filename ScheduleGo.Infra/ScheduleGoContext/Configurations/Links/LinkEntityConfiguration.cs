using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleGo.Shared.ScheduleGoContext.Entities;

namespace ScheduleGo.Infra.ScheduleGoContext.Configurations.Links
{
	public class LinkEntityConfiguration<TLinkEntity, TLeftEntity, TRightEntity> : IEntityTypeConfiguration<TLinkEntity> where TLinkEntity : LinkEntity<TLeftEntity, TRightEntity> where TLeftEntity : Entity where TRightEntity : Entity
	{
		public void Configure(EntityTypeBuilder<TLinkEntity> builder)
		{
			string leftPropertyCollumnName = $"{typeof(TLeftEntity).Name}Id";
			string rightPropertyCollumnName = $"{typeof(TRightEntity).Name}Id";

			builder.ToTable(typeof(TLinkEntity).Name);

			builder.Property<Guid>(leftPropertyCollumnName).HasColumnName(leftPropertyCollumnName).IsRequired();
			builder.Property<Guid>(rightPropertyCollumnName).HasColumnName(rightPropertyCollumnName).IsRequired();

			builder.HasKey(leftPropertyCollumnName, rightPropertyCollumnName);
		}
	}
}