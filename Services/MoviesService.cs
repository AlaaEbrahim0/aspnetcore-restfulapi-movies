using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace MoviesApi.Services
{
	public class MoviesService : IMoviesService
	{
		private readonly ApplicationDbContext context;

		public MoviesService(ApplicationDbContext context)
        {
			this.context = context;
		}

        public async Task<Movie> Create(Movie movie)
		{
			await context.AddAsync(movie);
			context.SaveChanges();

			return movie;
		}

		public Movie Delete(Movie movie)
		{
			context.Remove(movie);
			context.SaveChanges();

			return movie;
		}

		public async Task<IEnumerable<Movie>> GetAll(byte genreId = 0)
		{
			var movies = await context.Movies
				.Where(m => m.GenreId == genreId || genreId == 0)
				.OrderByDescending(m => m.Rate)
				.Include(m => m.Genre)
				.ToListAsync();

			return movies;
		}

		public async Task<Movie> GetById(int id)
		{
			var movie = await context.Movies.FindAsync(id);
			return movie;
		}

		public Movie Update(Movie movie)
		{
			context.Update(movie);
			context.SaveChanges();

			return movie;
		}
	}
}
