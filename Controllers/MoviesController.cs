using System.Runtime.CompilerServices;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;
using Microsoft.VisualBasic;
using MoviesApi.Migrations;
using MoviesApi.Models;
using MoviesApi.Services.Contracts;

namespace MoviesApi.Controllers
{
	[Authorize(Roles = "Admin")] 
    [Route("api/[controller]")]
	[ApiController]
	public class MoviesController : ControllerBase
	{
		private readonly IMoviesService moviesService;
		private readonly IGenresService genresService;
		private readonly IMapper mapper;
		private List<string> allowedExtensions = new List<String>
		{
			".jpg",
			".png"
		};

		private long maxAllowedPosterSize = 4194304;

		public MoviesController(IMoviesService moviesService, IGenresService genresService, IMapper mapper)
		{
			this.moviesService = moviesService;
			this.genresService = genresService;
			this.mapper = mapper;
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetAllAsync()
		{
			var movies = await moviesService.GetAll();

			var data = mapper.Map<IEnumerable<MovieDetailsDto>>(movies);

			return Ok(data);
		}
        [AllowAnonymous]
        [HttpGet("{id}")]
		public async Task<IActionResult> GetAsync(int id)
		{
			var movie = await moviesService.GetById(id);

			if (movie == null)
			{
				return NotFound();
			}

			var data = mapper.Map<MovieDetailsDto>(movie);

			return Ok(data);
		}

        [HttpGet("GetAllInGenre")]
		[AllowAnonymous]
		public async Task<IActionResult> GetAllInGenreAsync(byte genreId)
		{
			var movies = await moviesService.GetAll(genreId);

			var data = mapper.Map<IEnumerable<MovieDetailsDto>>(movies);

			return Ok(data);
		}

		[HttpPost]
        [Authorize(Roles = "Admin")]


        public async Task<IActionResult> CreateAsync([FromForm] MovieDto dto)
		{
			if (dto.Poster == null)
			{
				return BadRequest("Poster is required!");
			}

			if (!allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
			{
				return BadRequest("Only .png and .jpg images are allowed");
			}

			if (dto.Poster.Length > maxAllowedPosterSize)
			{
				return BadRequest("Max allowed size is 4MB!!");
			}

			var isValidGenre = await genresService.IsValidGenre(dto.GenreId);

			if (!isValidGenre)
			{
				return BadRequest("Invalid Genre ID");
			}

			using var dataStream = new MemoryStream();

			await dto.Poster.CopyToAsync(dataStream);

			var poster = dataStream.ToArray();

			var movie = mapper.Map<Movie>(dto);
			movie.Poster = poster;
			
			await moviesService.Create(movie);
			
			return Ok(movie);

		}

        [Authorize(Roles = "Admin")]
        [HttpPut]
		public async Task<IActionResult> UpdateAsync(int id, [FromForm] MovieDto dto)
		{

			var movie = await moviesService.GetById(id);

			if (movie == null)
			{
				return NotFound();
			}

			var isValidGenre = await genresService.IsValidGenre(dto.GenreId);

			if (!isValidGenre)
			{
				return BadRequest("Invalid Genre ID");
			}

			if (dto.Poster != null )
			{
				if (!allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
				{
					return BadRequest("Only .png and .jpg images are allowed");
				}

				if (dto.Poster.Length > maxAllowedPosterSize)
				{
					return BadRequest("Max allowed size is 4MB!!");
				}

				using var dataStream = new MemoryStream();

				await dto.Poster.CopyToAsync(dataStream);

				movie.Poster = dataStream.ToArray();

			}

			movie.Title = dto.Title;
			movie.StoryLine = dto.Storyline;
			movie.Year = dto.Year;
			movie.Rate = dto.Rate;
			movie.GenreId = dto.GenreId;

			moviesService.Update(movie);

			return Ok(movie);

		}

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAsync(int id)
		{
			var movie = await moviesService.GetById(id);
			if (movie == null)
			{
				return NotFound();
			}

			moviesService.Delete(movie);
			return Ok(movie);
		}
	}
}
