﻿// <auto-generated />
using System;
using Lemon.Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Lemon.Backend.Migrations
{
    [DbContext(typeof(LemonDbContext))]
    partial class LemonDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Lemon.Backend.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("e22e0709-bf31-44ec-ba27-f4158d796539"),
                            NickName = "John Ave",
                            PasswordHash = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                            PhoneNumber = "12345678901",
                            Salt = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
                        },
                        new
                        {
                            Id = new Guid("ce7d725b-09be-400a-9f51-0185dd98af74"),
                            Email = "Robert@mail.com",
                            NickName = "Robert St",
                            PasswordHash = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                            PhoneNumber = "12345678902",
                            Salt = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                            UserName = "Robert"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
