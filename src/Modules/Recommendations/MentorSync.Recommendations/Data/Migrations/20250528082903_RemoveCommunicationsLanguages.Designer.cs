﻿// <auto-generated />
using System;
using System.Collections.Generic;
using MentorSync.Recommendations.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MentorSync.Recommendations.Data.Migrations
{
    [DbContext(typeof(RecommendationsDbContext))]
    [Migration("20250528082903_RemoveCommunicationsLanguages")]
    partial class RemoveCommunicationsLanguages
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("recommendations")
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MentorSync.Recommendations.Domain.Interaction.MentorMenteeInteraction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("MenteeId")
                        .HasColumnType("integer");

                    b.Property<int>("MentorId")
                        .HasColumnType("integer");

                    b.Property<float>("Score")
                        .HasColumnType("real");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("MenteeMentorInteractions", "recommendations");
                });

            modelBuilder.Entity("MentorSync.Recommendations.Domain.Preferences.MenteePreference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("DesiredIndustries")
                        .HasColumnType("integer");

                    b.PrimitiveCollection<List<string>>("DesiredProgrammingLanguages")
                        .HasColumnType("text[]");

                    b.Property<int>("MenteeId")
                        .HasColumnType("integer");

                    b.Property<int?>("MinMentorExperienceYears")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("MenteePreferences", "recommendations");
                });

            modelBuilder.Entity("MentorSync.Recommendations.Domain.RecommendationResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<float>("CollaborativeScore")
                        .HasColumnType("real");

                    b.Property<float>("ContentBasedScore")
                        .HasColumnType("real");

                    b.Property<float>("FinalScore")
                        .HasColumnType("real");

                    b.Property<DateTime>("GeneratedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("MenteeId")
                        .HasColumnType("integer");

                    b.Property<int>("MentorId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("RecommendationResults", "recommendations");
                });

            modelBuilder.Entity("MentorSync.Recommendations.Domain.Tracking.MentorBookmark", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("BookmarkedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("MenteeId")
                        .HasColumnType("integer");

                    b.Property<int>("MentorId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("MentorBookmarks", "recommendations");
                });

            modelBuilder.Entity("MentorSync.Recommendations.Domain.Tracking.MentorViewEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("MenteeId")
                        .HasColumnType("integer");

                    b.Property<int>("MentorId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ViewedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("MentorViewEvents", "recommendations");
                });
#pragma warning restore 612, 618
        }
    }
}
