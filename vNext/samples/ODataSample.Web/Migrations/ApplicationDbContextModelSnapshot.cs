﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using ODataSample.Web.Models;

namespace ODataSample.Web.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rc2-20459")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ODataSample.Web.Models.Customer", b =>
                {
                    b.Property<int>("CustomerId");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.HasKey("CustomerId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("ODataSample.Web.Models.Product", b =>
                {
                    b.Property<int>("ProductId");

                    b.Property<int>("CustomerId");

                    b.Property<string>("Name");

                    b.Property<double>("Price");

                    b.HasKey("ProductId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ODataSample.Web.Models.Product", b =>
                {
                    b.HasOne("ODataSample.Web.Models.Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}