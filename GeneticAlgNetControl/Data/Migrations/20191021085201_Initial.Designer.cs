﻿// <auto-generated />
using GeneticAlgNetControl.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeneticAlgNetControl.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20191021085201_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0");

            modelBuilder.Entity("GeneticAlgNetControl.Data.Models.AlgorithmData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("AlgorithmRunId")
                        .HasColumnType("TEXT");

                    b.Property<string>("DrugTargetNodes")
                        .HasColumnType("TEXT");

                    b.Property<string>("Edges")
                        .HasColumnType("TEXT");

                    b.Property<string>("TargetNodes")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("AlgorithmData");
                });

            modelBuilder.Entity("GeneticAlgNetControl.Data.Models.AlgorithmParameters", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("AlgorithmRunId")
                        .HasColumnType("TEXT");

                    b.Property<int>("MaximumIterations")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaximumIterationsWithoutImprovement")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaximumPathLength")
                        .HasColumnType("INTEGER");

                    b.Property<double>("PercentageElite")
                        .HasColumnType("REAL");

                    b.Property<double>("PercentageRandom")
                        .HasColumnType("REAL");

                    b.Property<int>("PopulationSize")
                        .HasColumnType("INTEGER");

                    b.Property<double>("ProbabilityMutation")
                        .HasColumnType("REAL");

                    b.Property<int>("RandomGenesPerChromosome")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RandomSeed")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("AlgorithmParameters");
                });

            modelBuilder.Entity("GeneticAlgNetControl.Data.Models.AlgorithmRun", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("AlgorithmDataId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("AlgorithmParametersId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("CurrentIteration")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CurrentIterationWithoutImprovement")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DateTimeList")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AlgorithmDataId")
                        .IsUnique();

                    b.HasIndex("AlgorithmParametersId")
                        .IsUnique();

                    b.ToTable("AlgorithmRuns");
                });

            modelBuilder.Entity("GeneticAlgNetControl.Data.Models.AlgorithmRun", b =>
                {
                    b.HasOne("GeneticAlgNetControl.Data.Models.AlgorithmData", "AlgorithmData")
                        .WithOne("AlgorithmRun")
                        .HasForeignKey("GeneticAlgNetControl.Data.Models.AlgorithmRun", "AlgorithmDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GeneticAlgNetControl.Data.Models.AlgorithmParameters", "AlgorithmParameters")
                        .WithOne("AlgorithmRun")
                        .HasForeignKey("GeneticAlgNetControl.Data.Models.AlgorithmRun", "AlgorithmParametersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
