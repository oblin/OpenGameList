using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenGameList.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20161102073743_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("OpenGameList.Data.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnName("created_date");

                    b.Property<string>("DisplayName")
                        .HasColumnName("display_name");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("email");

                    b.Property<int>("Flags")
                        .HasColumnName("flags");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnName("last_modified_date");

                    b.Property<string>("Notes")
                        .HasColumnName("notes");

                    b.Property<int>("Type")
                        .HasColumnName("type");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnName("user_name")
                        .HasAnnotation("MaxLength", 128);

                    b.HasKey("Id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("OpenGameList.Data.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnName("created_date");

                    b.Property<int>("Flags")
                        .HasColumnName("flags");

                    b.Property<int>("ItemId")
                        .HasColumnName("item_id");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnName("last_modified_date");

                    b.Property<int?>("ParentId");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnName("text");

                    b.Property<int>("Type")
                        .HasColumnName("type");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("ParentId");

                    b.HasIndex("UserId");

                    b.ToTable("comments");
                });

            modelBuilder.Entity("OpenGameList.Data.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnName("created_date");

                    b.Property<string>("Description")
                        .HasColumnName("description");

                    b.Property<int>("Flags")
                        .HasColumnName("flags");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnName("last_modified_date");

                    b.Property<string>("Notes")
                        .HasColumnName("notes");

                    b.Property<string>("Text")
                        .HasColumnName("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("title");

                    b.Property<int>("Type")
                        .HasColumnName("type");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnName("user_id");

                    b.Property<int>("ViewCount")
                        .HasColumnName("view_count");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("items");
                });

            modelBuilder.Entity("OpenGameList.Data.Comment", b =>
                {
                    b.HasOne("OpenGameList.Data.Item", "Item")
                        .WithMany("Comments")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("OpenGameList.Data.Comment", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");

                    b.HasOne("OpenGameList.Data.ApplicationUser", "Author")
                        .WithMany("Comments")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("OpenGameList.Data.Item", b =>
                {
                    b.HasOne("OpenGameList.Data.ApplicationUser", "Author")
                        .WithMany("Items")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
