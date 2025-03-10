﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Alex_Engineering_Project_Management_Program.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20250215070037_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.2");

            modelBuilder.Entity("Project", b =>
                {
                    b.Property<string>("ProjectID")
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<string>("Client")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CompletionDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(34)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("ProjectID");

                    b.ToTable("Projects");

                    b.HasDiscriminator().HasValue("Project");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Stage", b =>
                {
                    b.Property<int>("StageName")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProjectID")
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("INTEGER");

                    b.HasKey("StageName", "ProjectID");

                    b.HasIndex("ProjectID");

                    b.ToTable("Stages");
                });

            modelBuilder.Entity("AutomotiveEngineeringProject", b =>
                {
                    b.HasBaseType("Project");

                    b.HasDiscriminator().HasValue("AutomotiveEngineeringProject");
                });

            modelBuilder.Entity("EngineeringDraftingProject", b =>
                {
                    b.HasBaseType("Project");

                    b.HasDiscriminator().HasValue("EngineeringDraftingProject");
                });

            modelBuilder.Entity("Stage", b =>
                {
                    b.HasOne("Project", null)
                        .WithMany("Stages")
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Project", b =>
                {
                    b.Navigation("Stages");
                });
#pragma warning restore 612, 618
        }
    }
}
