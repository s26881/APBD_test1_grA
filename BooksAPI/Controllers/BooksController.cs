using BooksAPI.Models;
using BooksAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BooksAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("{id}/editions")]
        public async Task<IActionResult> GetEditions(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var editions = await _bookService.GetEditions(id);
                return Ok(editions);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddBookModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdBookId = await _bookService.CreateAsync(model);
                return CreatedAtAction(nameof(GetEditions), createdBookId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
