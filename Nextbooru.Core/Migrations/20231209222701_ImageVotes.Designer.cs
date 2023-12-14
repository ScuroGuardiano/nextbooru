﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nextbooru.Core.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Nextbooru.Core.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231209222701_ImageVotes")]
    partial class ImageVotes
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ImageTag", b =>
                {
                    b.Property<long>("ImagesId")
                        .HasColumnType("bigint")
                        .HasColumnName("images_id");

                    b.Property<int>("TagsId")
                        .HasColumnType("integer")
                        .HasColumnName("tags_id");

                    b.HasKey("ImagesId", "TagsId")
                        .HasName("pk_image_tag");

                    b.HasIndex("TagsId")
                        .HasDatabaseName("ix_image_tag_tags_id");

                    b.ToTable("image_tag", (string)null);
                });

            modelBuilder.Entity("Nextbooru.Auth.Models.Session", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<bool>("IsValid")
                        .HasColumnType("boolean")
                        .HasColumnName("is_valid");

                    b.Property<DateTime>("LastAccess")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_access");

                    b.Property<string>("LastIP")
                        .HasColumnType("text")
                        .HasColumnName("last_ip");

                    b.Property<string>("LoggedInIP")
                        .HasColumnType("text")
                        .HasColumnName("logged_in_ip");

                    b.Property<string>("UserAgent")
                        .HasColumnType("text")
                        .HasColumnName("user_agent");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_sessions");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_sessions_user_id");

                    b.ToTable("sessions", (string)null);
                });

            modelBuilder.Entity("Nextbooru.Auth.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime?>("BannedUntil")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("banned_until");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("display_name");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("hashed_password");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean")
                        .HasColumnName("is_admin");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasDatabaseName("ix_users_username");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Nextbooru.Core.Models.Image", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("ContentType")
                        .HasColumnType("text")
                        .HasColumnName("content_type");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("Extension")
                        .HasColumnType("text")
                        .HasColumnName("extension");

                    b.Property<int>("Height")
                        .HasColumnType("integer")
                        .HasColumnName("height");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("boolean")
                        .HasColumnName("is_public");

                    b.Property<int>("Score")
                        .HasColumnType("integer")
                        .HasColumnName("score");

                    b.Property<long>("SizeInBytes")
                        .HasColumnType("bigint")
                        .HasColumnName("size_in_bytes");

                    b.Property<string>("Source")
                        .HasColumnType("text")
                        .HasColumnName("source");

                    b.Property<string>("StoreFileId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("store_file_id");

                    b.Property<List<int>>("TagsArr")
                        .IsRequired()
                        .HasColumnType("integer[]")
                        .HasColumnName("tags_arr");

                    b.Property<string>("Title")
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("NOW()");

                    b.Property<Guid>("UploadedById")
                        .HasColumnType("uuid")
                        .HasColumnName("uploaded_by_id");

                    b.Property<int>("Width")
                        .HasColumnType("integer")
                        .HasColumnName("width");

                    b.HasKey("Id")
                        .HasName("pk_images");

                    b.HasIndex("TagsArr")
                        .HasDatabaseName("ix_images_tags_arr");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("TagsArr"), "GIN");

                    b.HasIndex("UploadedById")
                        .HasDatabaseName("ix_images_uploaded_by_id");

                    b.ToTable("images", (string)null);
                });

            modelBuilder.Entity("Nextbooru.Core.Models.ImageVariant", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("ContentType")
                        .HasColumnType("text")
                        .HasColumnName("content_type");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("Extension")
                        .HasColumnType("text")
                        .HasColumnName("extension");

                    b.Property<int>("Height")
                        .HasColumnType("integer")
                        .HasColumnName("height");

                    b.Property<long>("ImageId")
                        .HasColumnType("bigint")
                        .HasColumnName("image_id");

                    b.Property<long>("SizeInBytes")
                        .HasColumnType("bigint")
                        .HasColumnName("size_in_bytes");

                    b.Property<string>("StoreFileId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("store_file_id");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("NOW()");

                    b.Property<int>("VariantMode")
                        .HasColumnType("integer")
                        .HasColumnName("variant_mode");

                    b.Property<int>("Width")
                        .HasColumnType("integer")
                        .HasColumnName("width");

                    b.HasKey("Id")
                        .HasName("pk_image_variants");

                    b.HasIndex("ImageId")
                        .HasDatabaseName("ix_image_variants_image_id");

                    b.ToTable("image_variants", (string)null);
                });

            modelBuilder.Entity("Nextbooru.Core.Models.ImageVote", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("NOW()");

                    b.Property<long>("ImageId")
                        .HasColumnType("bigint")
                        .HasColumnName("image_id");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("NOW()");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<int>("VoteScore")
                        .HasColumnType("integer")
                        .HasColumnName("vote_score");

                    b.HasKey("Id")
                        .HasName("pk_image_votes");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_image_votes_user_id");

                    b.HasIndex("ImageId", "UserId")
                        .IsUnique()
                        .HasDatabaseName("ix_image_votes_image_id_user_id");

                    b.ToTable("image_votes", (string)null);
                });

            modelBuilder.Entity("Nextbooru.Core.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("NOW()");

                    b.Property<int>("ImagesCount")
                        .HasColumnType("integer")
                        .HasColumnName("images_count");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("TagType")
                        .HasColumnType("integer")
                        .HasColumnName("tag_type");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("NOW()");

                    b.HasKey("Id")
                        .HasName("pk_tags");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_tags_name");

                    b.ToTable("tags", (string)null);
                });

            modelBuilder.Entity("ImageTag", b =>
                {
                    b.HasOne("Nextbooru.Core.Models.Image", null)
                        .WithMany()
                        .HasForeignKey("ImagesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_image_tag_images_images_id");

                    b.HasOne("Nextbooru.Core.Models.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_image_tag_tags_tags_id");
                });

            modelBuilder.Entity("Nextbooru.Auth.Models.Session", b =>
                {
                    b.HasOne("Nextbooru.Auth.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_sessions_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Nextbooru.Core.Models.Image", b =>
                {
                    b.HasOne("Nextbooru.Auth.Models.User", "UploadedBy")
                        .WithMany()
                        .HasForeignKey("UploadedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_images_users_uploaded_by_id");

                    b.Navigation("UploadedBy");
                });

            modelBuilder.Entity("Nextbooru.Core.Models.ImageVariant", b =>
                {
                    b.HasOne("Nextbooru.Core.Models.Image", "Image")
                        .WithMany("Variants")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_image_variants_images_image_id");

                    b.Navigation("Image");
                });

            modelBuilder.Entity("Nextbooru.Core.Models.ImageVote", b =>
                {
                    b.HasOne("Nextbooru.Core.Models.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_image_votes_images_image_id");

                    b.HasOne("Nextbooru.Auth.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_image_votes_users_user_id");

                    b.Navigation("Image");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Nextbooru.Core.Models.Image", b =>
                {
                    b.Navigation("Variants");
                });
#pragma warning restore 612, 618
        }
    }
}