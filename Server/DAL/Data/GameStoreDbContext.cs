﻿using DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    public class GameStoreDbContext : IdentityDbContext<User>
    {
        public GameStoreDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameGenre>()
                        .HasKey(x => new
                        {
                            x.GameId,
                            x.GenreId
                        });

            modelBuilder.Entity<Comment>()
                        .HasMany(rc => rc.Replies)
                        .WithOne(pc => pc.ParentComment)
                        .HasForeignKey(pc => pc.ReplieId)
                        .Metadata.DeleteBehavior = DeleteBehavior.Restrict;

            modelBuilder.Entity<Genre>()
                        .HasMany(g => g.SubGenres)
                        .WithOne(mg => mg.MainGenre)
                        .HasForeignKey(fk => fk.ParentId)
                        .Metadata.DeleteBehavior = DeleteBehavior.Restrict;
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Comment>()
                        .Property(c => c.Body)
                        .HasMaxLength(600);

            modelBuilder.Entity<Comment>();
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameGenre> GameGenre { get; set; }
        public DbSet<GameImage> GameImages { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
