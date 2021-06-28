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
            this._bookManager = bookManager;
        }

        [HttpGet]
        public IActionResult GetAllBooks([FromQuery] Filter filter)
        {
            List<BookResource> bookResources = _bookManager.GetAll(filter);
            return Ok(bookResources);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Details([FromRoute] int Id)
        {
            BookResource bookResource = await _bookManager.GetByIdAsync(Id);
            return Ok(bookResource);
        }

        [HttpGet("ExtraDetails/{Id}")]
        public async Task<IActionResult> GetBookWithAuthorsAndPublisher([FromQuery] Filter bookFilter, [FromRoute] int Id)
        {
            BookResource bookResource = await _bookManager.GetBookWithAuthorsAndPublisher(bookFilter, Id);
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

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            await _bookManager.Delete(Id);
            return Ok();
        }


    }
}
