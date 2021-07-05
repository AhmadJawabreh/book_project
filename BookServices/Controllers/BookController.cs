using BusinessLogic;
using Filters;
using Microsoft.AspNetCore.Mvc;
using Models;
using Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("Book")]
    [ApiController]
    public class BookController : Controller
    {
        private readonly IBookManager _bookManager;

        public BookController(IBookManager bookManager)
        {
            _bookManager = bookManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooksAsync([FromQuery] Filter filter)
        {
            List<BookResource> bookResources = await _bookManager.GetAllAsync(filter);
            return Ok(bookResources);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Details([FromRoute] int id)
        {
            BookResource bookResource = await _bookManager.GetByIdAsync(id);
            return Ok(bookResource);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookModel bookModel)
        {
            BookResource bookResource = await _bookManager.InsertAsync(bookModel);
            return Ok(bookResource);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] BookModel bookModel)
        {
            BookResource bookResource = await _bookManager.UpdateAsync(bookModel);
            return Ok(bookResource);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _bookManager.Delete(id);
            return Ok();
        }
    }
}
