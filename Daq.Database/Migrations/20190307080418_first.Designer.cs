﻿// <auto-generated />
using System;
using DAQ.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Daq.Database.Migrations
{
    [DbContext(typeof(OeedbContext))]
    [Migration("20190307080418_first")]
    partial class first
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity("DAQ.Database.StatusDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AlarmInfoId");

                    b.Property<TimeSpan>("Span");

                    b.Property<int?>("StatusInfoId");

                    b.Property<DateTime>("Time");

                    b.HasKey("Id");

                    b.HasIndex("StatusInfoId");

                    b.ToTable("Alarms");
                });

            modelBuilder.Entity("DAQ.Database.StatusInfoDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AlarmContent");

                    b.Property<int>("AlarmIndex");

                    b.Property<int>("StationId");

                    b.HasKey("Id");

                    b.ToTable("AlarmInfos");
                });

            modelBuilder.Entity("DAQ.Database.StatusDto", b =>
                {
                    b.HasOne("DAQ.Database.StatusInfoDto", "StatusInfo")
                        .WithMany()
                        .HasForeignKey("StatusInfoId");
                });
#pragma warning restore 612, 618
        }
    }
}
