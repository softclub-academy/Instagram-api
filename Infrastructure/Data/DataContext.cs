﻿using Domain.Entities;
using Domain.Entities.Post;
using Domain.Entities.User;
using Domain.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Infrastructure.Data;

public class DataContext : IdentityDbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    [Obsolete("Obsolete")]
    public DataContext()
    {
        NpgsqlConnection.GlobalTypeMapper.MapEnum<Gender>();
        NpgsqlConnection.GlobalTypeMapper.MapEnum<Active>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<Gender>();
        modelBuilder.HasPostgresEnum<Active>();
        modelBuilder.Entity<FollowingRelationShip>()
            .HasOne<User>(u => u.User)
            .WithMany(f => f.FollowingRelationShips)
            .HasForeignKey(u => u.UserId);
        modelBuilder.Entity<User>()
            .HasIndex(u => u.UserName)
            .IsUnique();
        modelBuilder.Entity<UserProfile>()
            .HasIndex(u => u.UserId)
            .IsUnique();
        modelBuilder.Entity<Tag>()
            .HasIndex(u => u.TagName)
            .IsUnique();
        modelBuilder.Entity<Category>()
            .HasIndex(u => u.CategoryName)
            .IsUnique();
        modelBuilder.Entity<StatUserId>()
            .HasIndex(s => s.UserId)
            .IsUnique();
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostCategory> PostCategories { get; set; }
    public DbSet<PostComment> PostComments { get; set; }
    public DbSet<PostFavorite> PostFavorites { get; set; }
    public DbSet<PostStat> PostStats { get; set; }
    public DbSet<PostTag> PostTags { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<ExternalAccount> ExternalAccounts { get; set; }
    public DbSet<FollowingRelationShip> FollowingRelationShips { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<UserSetting> UserSettings { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<StatUserId> StatUserIds { get; set; }
    public DbSet<PostView> PostViews { get; set; }
    public DbSet<PostViewUser> PostViewUsers { get; set; }
}