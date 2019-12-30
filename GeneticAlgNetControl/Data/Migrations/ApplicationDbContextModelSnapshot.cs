﻿// <auto-generated />
using System;
using GeneticAlgNetControl.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeneticAlgNetControl.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0");

            modelBuilder.Entity("GeneticAlgNetControl.Data.Models.Analysis", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("CurrentIteration")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CurrentIterationWithoutImprovement")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DateTimeEnded")
                        .HasColumnType("TEXT");

                    b.Property<string>("DateTimeIntervals")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateTimeStarted")
                        .HasColumnType("TEXT");

                    b.Property<string>("Edges")
                        .HasColumnType("TEXT");

                    b.Property<string>("HistoricAverageFitness")
                        .HasColumnType("TEXT");

                    b.Property<string>("HistoricBestFitness")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nodes")
                        .HasColumnType("TEXT");

                    b.Property<int>("NumberOfEdges")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumberOfNodes")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumberOfPreferredNodes")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumberOfTargetNodes")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Parameters")
                        .HasColumnType("TEXT");

                    b.Property<string>("Population")
                        .HasColumnType("TEXT");

                    b.Property<string>("PreferredNodes")
                        .HasColumnType("TEXT");

                    b.Property<string>("Solutions")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TargetNodes")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Analyses");
                });
#pragma warning restore 612, 618
        }
    }
}
