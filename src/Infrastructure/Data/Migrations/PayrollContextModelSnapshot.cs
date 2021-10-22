﻿// <auto-generated />
using System;
using Assessment.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    [DbContext(typeof(PayrollContext))]
    partial class PayrollContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0-rc.2.21480.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Assessment.ApplicationCore.Entities.Contract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("MaxAllowedSickLeaveHours")
                        .HasColumnType("int");

                    b.Property<int>("MaxAllowedVacationHours")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PayMethod")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Wage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("WorkingWeekHours")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("Assessment.ApplicationCore.Entities.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Assessment.ApplicationCore.Entities.PaymentFactor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PaymentFactorType")
                        .HasColumnType("int");

                    b.Property<int>("PaymentHistoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PaymentHistoryId");

                    b.ToTable("PaymentFactor");
                });

            modelBuilder.Entity("Assessment.ApplicationCore.Entities.PaymentHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CalculationTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("GrossIncome")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("NetIncome")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("PaymentTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Tax")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TimesheetId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TimesheetId");

                    b.ToTable("PaymentHistories");
                });

            modelBuilder.Entity("Assessment.ApplicationCore.Entities.TaxBracket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("High")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Low")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Rate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TaxReferenceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TaxReferenceId");

                    b.ToTable("TaxBrackets");
                });

            modelBuilder.Entity("Assessment.ApplicationCore.Entities.TaxReference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("TaxReferences");
                });

            modelBuilder.Entity("Assessment.ApplicationCore.Entities.Timesheet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ContractId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FromDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPayed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("SickLeaveHours")
                        .HasColumnType("int");

                    b.Property<DateTime>("ToDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("VacationHours")
                        .HasColumnType("int");

                    b.Property<int>("WorkingHours")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ContractId");

                    b.ToTable("Timesheets");
                });

            modelBuilder.Entity("Assessment.ApplicationCore.Entities.Contract", b =>
                {
                    b.HasOne("Assessment.ApplicationCore.Entities.Employee", "Employee")
                        .WithMany("PaymentFactors")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Assessment.ApplicationCore.Entities.PaymentFactor", b =>
                {
                    b.HasOne("Assessment.ApplicationCore.Entities.PaymentHistory", "PaymentHistory")
                        .WithMany("PaymentFactors")
                        .HasForeignKey("PaymentHistoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PaymentHistory");
                });

            modelBuilder.Entity("Assessment.ApplicationCore.Entities.PaymentHistory", b =>
                {
                    b.HasOne("Assessment.ApplicationCore.Entities.Timesheet", "Timesheet")
                        .WithMany()
                        .HasForeignKey("TimesheetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Timesheet");
                });

            modelBuilder.Entity("Assessment.ApplicationCore.Entities.TaxBracket", b =>
                {
                    b.HasOne("Assessment.ApplicationCore.Entities.TaxReference", "TaxReference")
                        .WithMany("TaxBrackets")
                        .HasForeignKey("TaxReferenceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TaxReference");
                });

            modelBuilder.Entity("Assessment.ApplicationCore.Entities.Timesheet", b =>
                {
                    b.HasOne("Assessment.ApplicationCore.Entities.Contract", "Contract")
                        .WithMany()
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contract");
                });

            modelBuilder.Entity("Assessment.ApplicationCore.Entities.Employee", b =>
                {
                    b.Navigation("PaymentFactors");
                });

            modelBuilder.Entity("Assessment.ApplicationCore.Entities.PaymentHistory", b =>
                {
                    b.Navigation("PaymentFactors");
                });

            modelBuilder.Entity("Assessment.ApplicationCore.Entities.TaxReference", b =>
                {
                    b.Navigation("TaxBrackets");
                });
#pragma warning restore 612, 618
        }
    }
}
