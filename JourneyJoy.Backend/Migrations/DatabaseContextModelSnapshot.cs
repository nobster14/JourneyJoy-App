﻿// <auto-generated />
using System;
using JourneyJoy.Model.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JourneyJoy.Backend.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("JourneyJoy.Model.Database.Tables.Attraction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsStartPoint")
                        .HasColumnType("bit");

                    b.Property<bool>("IsUrl")
                        .HasColumnType("bit");

                    b.Property<int>("LocationType")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("OpenHours")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prices")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("TimeNeeded")
                        .HasColumnType("float");

                    b.Property<Guid>("TripId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TripId");

                    b.ToTable("Attractions");
                });

            modelBuilder.Entity("JourneyJoy.Model.Database.Tables.Route", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SerializedAttractionsIds")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StartDay")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("JourneyJoy.Model.Database.Tables.Trip", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasMaxLength(5000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("RouteId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RouteId");

                    b.HasIndex("UserId");

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("JourneyJoy.Model.Database.Tables.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("JourneyJoy.Model.Database.Tables.Attraction", b =>
                {
                    b.HasOne("JourneyJoy.Model.Database.Tables.Trip", "Trip")
                        .WithMany("Attractions")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("JourneyJoy.Model.Database.Tables.Location", "Location", b1 =>
                        {
                            b1.Property<Guid>("AttractionId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Address")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("City")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Country")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<double>("Latitude")
                                .HasColumnType("float");

                            b1.Property<double>("Longitude")
                                .HasColumnType("float");

                            b1.Property<string>("Phone")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Postalcode")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("State")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Street1")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("AttractionId");

                            b1.ToTable("Locations");

                            b1.WithOwner()
                                .HasForeignKey("AttractionId");
                        });

                    b.Navigation("Location")
                        .IsRequired();

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("JourneyJoy.Model.Database.Tables.Trip", b =>
                {
                    b.HasOne("JourneyJoy.Model.Database.Tables.Route", "Route")
                        .WithMany()
                        .HasForeignKey("RouteId");

                    b.HasOne("JourneyJoy.Model.Database.Tables.User", "User")
                        .WithMany("UserTrips")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Route");

                    b.Navigation("User");
                });

            modelBuilder.Entity("JourneyJoy.Model.Database.Tables.Trip", b =>
                {
                    b.Navigation("Attractions");
                });

            modelBuilder.Entity("JourneyJoy.Model.Database.Tables.User", b =>
                {
                    b.Navigation("UserTrips");
                });
#pragma warning restore 612, 618
        }
    }
}
