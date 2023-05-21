using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace MoviesApi.Models
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Movie>()
				.HasOne(g => g.Genre)
				.WithMany()
				.HasForeignKey(f => f.GenreId);	
		}

		public DbSet<Genre> Genres { get; set; }
		public DbSet<Movie> Movies { get; set; }
	}
}
