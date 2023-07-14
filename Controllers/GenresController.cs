using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;
using MoviesApi.Models;
using MoviesApi.Services.Contracts;

namespace WebApplication1.Controllers
{
	[Authorize(Roles = "Admin")]
	[Route("api/[controller]")]
	[ApiController]
	public class GenresController : ControllerBase
	{
		private readonly IGenresService genresService;

		public GenresController(IGenresService genresService)
		{
			this.genresService = genresService;
		}

		[AllowAnonymous]
		[HttpGet]
		public async Task<IActionResult> GetAllAsync()
		{
			var genres = await genresService.GetAll();
			return Ok (genres);
		}

		[HttpPost]
		public async Task<IActionResult> CreateAsync(GenreDto dto)
		{
			var genre = new Genre
			{
				Name = dto.Name
			};
			await genresService.Create(genre);

			return Ok(genre);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateAsync (byte id, [FromBody]GenreDto dto)
		{
			var genre = await genresService.GetById(id);
			if (genre == null)
			{
				return NotFound($"No genre was found with ID: {id}");
			}

			genre.Name = dto.Name;
			genresService.Update(genre);
			 
			return Ok(genre);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAsync (byte id)
		{
			var genre = await genresService.GetById(id);
			if (genre == null)
			{
				return NotFound($"No genre was found with ID: {id}");
			}

			genresService.Delete(genre);	

			return Ok(genre);
		}
	}
}
