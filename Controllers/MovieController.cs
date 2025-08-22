using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_Hub.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie_Hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieCollectionContext _ctx;

        public MoviesController(MovieCollectionContext ctx) => _ctx = ctx;

        // GET: api/movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> Get() =>
            await _ctx.Movies.ToListAsync();

        // GET: api/movies/search?name=Inception&genre=Sci-Fi
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Movie>>> Search(string? name, string? genre)
        {
            var q = _ctx.Movies.AsQueryable();
            if (!string.IsNullOrWhiteSpace(name)) q = q.Where(m => m.Name.Contains(name));
            if (!string.IsNullOrWhiteSpace(genre)) q = q.Where(m => m.Genre.Contains(genre));
            return await q.ToListAsync();
        }

        // GET: api/movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> Get(int id)
        {
            var m = await _ctx.Movies.FindAsync(id);
            if (m == null) return NotFound();
            return m;
        }

        // POST: api/movies
        [HttpPost]
        public async Task<ActionResult<Movie>> Create(Movie movie)
        {
            movie.DateAdded = DateTime.UtcNow;
            _ctx.Movies.Add(movie);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = movie.Id }, movie);
        }

        // PUT: api/movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Movie movie)
        {
            if (id != movie.Id) return BadRequest();
            _ctx.Entry(movie).State = EntityState.Modified;
            try
            {
                await _ctx.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!_ctx.Movies.Any(e => e.Id == id))
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: api/movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _ctx.Movies.FindAsync(id);
            if (movie == null) return NotFound();
            _ctx.Movies.Remove(movie);
            await _ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}
