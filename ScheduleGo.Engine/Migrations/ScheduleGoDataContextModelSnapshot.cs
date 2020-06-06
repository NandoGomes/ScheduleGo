﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ScheduleGo.Infra.ScheduleGoContext.DataContexts;

namespace ScheduleGo.Engine.Migrations
{
    [DbContext(typeof(ScheduleGoDataContext))]
    partial class ScheduleGoDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ScheduleGo.Domain.ScheduleGoContext.Entities.Classroom", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<Guid>("ClassroomTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ClassroomTypeId");

                    b.ToTable("Classrooms");
                });

            modelBuilder.Entity("ScheduleGo.Domain.ScheduleGoContext.Entities.ClassroomType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ClassroomTypes");
                });

            modelBuilder.Entity("ScheduleGo.Domain.ScheduleGoContext.Entities.Course", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("NeededClassroomTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("StudentsCount")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("WeeklyWorkload")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.HasIndex("NeededClassroomTypeId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("ScheduleGo.Domain.ScheduleGoContext.Entities.Links.ClassroomTag", b =>
                {
                    b.Property<Guid>("LeftId")
                        .HasColumnName("Classroom")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RightId")
                        .HasColumnName("Tag")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LeftId", "RightId");

                    b.HasIndex("RightId");

                    b.ToTable("ClassroomTag");
                });

            modelBuilder.Entity("ScheduleGo.Domain.ScheduleGoContext.Entities.Links.ClassroomTimePeriod", b =>
                {
                    b.Property<Guid>("LeftId")
                        .HasColumnName("Classroom")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RightId")
                        .HasColumnName("TimePeriod")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LeftId", "RightId");

                    b.HasIndex("RightId");

                    b.ToTable("ClassroomTimePeriod");
                });

            modelBuilder.Entity("ScheduleGo.Domain.ScheduleGoContext.Entities.Links.CourseTag", b =>
                {
                    b.Property<Guid>("LeftId")
                        .HasColumnName("Course")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RightId")
                        .HasColumnName("Tag")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LeftId", "RightId");

                    b.HasIndex("RightId");

                    b.ToTable("CourseTag");
                });

            modelBuilder.Entity("ScheduleGo.Domain.ScheduleGoContext.Entities.Links.CourseTimePeriod", b =>
                {
                    b.Property<Guid>("LeftId")
                        .HasColumnName("Course")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RightId")
                        .HasColumnName("TimePeriod")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LeftId", "RightId");

                    b.HasIndex("RightId");

                    b.ToTable("CourseTimePeriod");
                });

            modelBuilder.Entity("ScheduleGo.Domain.ScheduleGoContext.Entities.Links.TeacherAvailablePeriod", b =>
                {
                    b.Property<Guid>("LeftId")
                        .HasColumnName("Teacher")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RightId")
                        .HasColumnName("TimePeriod")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LeftId", "RightId");

                    b.HasIndex("RightId");

                    b.ToTable("TeacherAvailablePeriod");
                });

            modelBuilder.Entity("ScheduleGo.Domain.ScheduleGoContext.Entities.Links.TeacherPreferredCourse", b =>
                {
                    b.Property<Guid>("LeftId")
                        .HasColumnName("Teacher")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RightId")
                        .HasColumnName("Course")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LeftId", "RightId");

                    b.HasIndex("RightId");

                    b.ToTable("TeacherPreferredCourse");
                });

            modelBuilder.Entity("ScheduleGo.Domain.ScheduleGoContext.Entities.Links.TeacherPreferredPeriod", b =>
                {
                    b.Property<Guid>("LeftId")
                        .HasColumnName("Teacher")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RightId")
                        .HasColumnName("TimePeriod")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LeftId", "RightId");

                    b.HasIndex("RightId");

                    b.ToTable("TeacherPreferredPeriod");
                });

            modelBuilder.Entity("ScheduleGo.Domain.ScheduleGoContext.Entities.Links.TeacherQualifiedCourse", b =>
                {
                    b.Property<Guid>("LeftId")
                        .HasColumnName("Teacher")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RightId")
                        .HasColumnName("Course")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LeftId", "RightId");

                    b.HasIndex("RightId");

                    b.ToTable("TeacherQualifiedCourse");
                });

            modelBuilder.Entity("ScheduleGo.Domain.ScheduleGoContext.Entities.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("ScheduleGo.Domain.ScheduleGoContext.Entities.Teacher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("ScheduleGo.Domain.ScheduleGoContext.Entities.TimePeriod", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("End")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("Start")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.ToTable("TimePeriods");
                });

            modelBuilder.Entity("ScheduleGo.Domain.ScheduleGoContext.Entities.Classroom", b =>
                {
                    b.HasOne("ScheduleGo.Domain.ScheduleGoContext.Entities.ClassroomType", "ClassroomType")
                        .WithMany()
                        .HasForeignKey("ClassroomTypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("ScheduleGo.Domain.ScheduleGoContext.Entities.Course", b =>
                {
                    b.HasOne("ScheduleGo.Domain.ScheduleGoContext.Entities.ClassroomType", "NeededClassroomType")
                        .WithMany()
                        .HasForeignKey("NeededClassroomTypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("ScheduleGo.Domain.ScheduleGoContext.Entities.Links.ClassroomTag", b =>
                {
                    b.HasOne("ScheduleGo.Domain.ScheduleGoContext.Entities.Classroom", "Left")
                        .WithMany()
                        .HasForeignKey("LeftId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ScheduleGo.Domain.ScheduleGoContext.Entities.Tag", "Right")
                        .WithMany()
                        .HasForeignKey("RightId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("ScheduleGo.Domain.ScheduleGoContext.Entities.Links.ClassroomTimePeriod", b =>
                {
                    b.HasOne("ScheduleGo.Domain.ScheduleGoContext.Entities.Classroom", "Left")
                        .WithMany()
                        .HasForeignKey("LeftId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ScheduleGo.Domain.ScheduleGoContext.Entities.TimePeriod", "Right")
                        .WithMany()
                        .HasForeignKey("RightId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("ScheduleGo.Domain.ScheduleGoContext.Entities.Links.CourseTag", b =>
                {
                    b.HasOne("ScheduleGo.Domain.ScheduleGoContext.Entities.Course", "Left")
                        .WithMany()
                        .HasForeignKey("LeftId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ScheduleGo.Domain.ScheduleGoContext.Entities.Tag", "Right")
                        .WithMany()
                        .HasForeignKey("RightId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("ScheduleGo.Domain.ScheduleGoContext.Entities.Links.CourseTimePeriod", b =>
                {
                    b.HasOne("ScheduleGo.Domain.ScheduleGoContext.Entities.Course", "Left")
                        .WithMany()
                        .HasForeignKey("LeftId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ScheduleGo.Domain.ScheduleGoContext.Entities.TimePeriod", "Right")
                        .WithMany()
                        .HasForeignKey("RightId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("ScheduleGo.Domain.ScheduleGoContext.Entities.Links.TeacherAvailablePeriod", b =>
                {
                    b.HasOne("ScheduleGo.Domain.ScheduleGoContext.Entities.Teacher", "Left")
                        .WithMany()
                        .HasForeignKey("LeftId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ScheduleGo.Domain.ScheduleGoContext.Entities.TimePeriod", "Right")
                        .WithMany()
                        .HasForeignKey("RightId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("ScheduleGo.Domain.ScheduleGoContext.Entities.Links.TeacherPreferredCourse", b =>
                {
                    b.HasOne("ScheduleGo.Domain.ScheduleGoContext.Entities.Teacher", "Left")
                        .WithMany()
                        .HasForeignKey("LeftId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ScheduleGo.Domain.ScheduleGoContext.Entities.Course", "Right")
                        .WithMany()
                        .HasForeignKey("RightId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("ScheduleGo.Domain.ScheduleGoContext.Entities.Links.TeacherPreferredPeriod", b =>
                {
                    b.HasOne("ScheduleGo.Domain.ScheduleGoContext.Entities.Teacher", "Left")
                        .WithMany()
                        .HasForeignKey("LeftId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ScheduleGo.Domain.ScheduleGoContext.Entities.TimePeriod", "Right")
                        .WithMany()
                        .HasForeignKey("RightId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("ScheduleGo.Domain.ScheduleGoContext.Entities.Links.TeacherQualifiedCourse", b =>
                {
                    b.HasOne("ScheduleGo.Domain.ScheduleGoContext.Entities.Teacher", "Left")
                        .WithMany()
                        .HasForeignKey("LeftId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ScheduleGo.Domain.ScheduleGoContext.Entities.Course", "Right")
                        .WithMany()
                        .HasForeignKey("RightId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
