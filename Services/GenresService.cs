using Microsoft.EntityFrameworkCore;
using MoviesApi.Services.Contracts;

namespace MoviesApi.Services
{
    public class GenresService : IGenresService
	{
		private readonly ApplicationDbContext context;

		public GenresService(ApplicationDbContext context)
        {
			this.context = context;
		}

        public async Task<Genre> Create(Genre genre)
		{
			await context.Genres.AddAsync(genre);
			context.SaveChanges();

			return genre;
		}

		public Genre Delete(Genre genre)
		{
			context.Remove(genre);
			context.SaveChanges();

			return genre;
		}

		public async Task<IEnumerable<Genre>> GetAll()
		{
			var genres = await context.Genres.OrderBy(x => x.Id).ToListAsync();
			return genres;
		}

		public async Task<Genre> GetById(byte id)
		{
			var genre = await context.Genres.SingleOrDefaultAsync(g => g.Id == id);
			return genre;
		}

		public async Task<bool> IsValidGenre(byte id)
		{
			return await context.Genres.AnyAsync(x => x.Id == id);
		}

		public Genre Update(Genre genre)
		{
			context.Update(genre);
			context.SaveChanges();

			return genre;
		}

		
	}
}
