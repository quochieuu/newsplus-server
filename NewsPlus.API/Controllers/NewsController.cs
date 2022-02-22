using Microsoft.AspNetCore.Mvc;
using NewsPlus.Common;
using NewsPlus.Data.ViewModel;
using NewsPlus.Infrastructure;

namespace NewsPlus.API.Controllers
{
    [Route("api/news")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsRepository _repo;

        public NewsController(INewsRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _repo.GetAll());
        }

        [HttpGet("filter")]
        public async Task<ActionResult> GetAllPaging(string? filter, int pageIndex, int pageSize)
        {
            return Ok(await _repo.GetAllPaging(filter, pageIndex, pageSize));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {

            var item = await _repo.GetById(id);

            if (item == null)
            {
                return NotFound(new ApiNotFoundResponse($"News with id: {id} is not found"));
            }

            return Ok(item);
        }

        [HttpGet("detail/{slug}")]
        public async Task<ActionResult> GetBySlug(string slug)
        {

            var item = await _repo.GetBySlug(slug);

            if (item == null)
            {
                return NotFound(new ApiNotFoundResponse($"News with slug: {slug} is not found"));
            }

            return Ok(item);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Post(CreateNewsViewModel model)
        {
            var result = await _repo.Create(model);

            if (result.Result > 0)
            {
                return RedirectToAction(nameof(GetById), new { id = result.Id });
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Create news failed"));
            }
        }


        [HttpPost("create-image/{newsId}")]
        public async Task<IActionResult> PostImage([FromForm] CreateNewsImageViewModel model, Guid newsId)
        {
            var result = await _repo.CreateImage(model, newsId);

            if (result.Result > 0)
            {
                return RedirectToAction(nameof(GetById), new { id = result.Id });
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Create news image failed"));
            }
        }


        [HttpPut("update/{id}")]
        public async Task<IActionResult> Put(UpdateNewsViewModel model, Guid id)
        {
            var item = await _repo.GetById(id);
            if (item == null)
                return NotFound(new ApiNotFoundResponse($"News with id: {id} is not found"));

            var result = await _repo.Update(id, model);

            if (result.Result > 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Update news failed"));
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _repo.GetById(id);

            if (item == null)
                return NotFound(new ApiNotFoundResponse($"News with id: {id} is not found"));

            var result = await _repo.Delete(id);

            if (result > 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Delete news failed"));
            }

        }

    }
}
