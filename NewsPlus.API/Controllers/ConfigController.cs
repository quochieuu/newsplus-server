using Microsoft.AspNetCore.Mvc;
using NewsPlus.Common;
using NewsPlus.Data.ViewModel;
using NewsPlus.Infrastructure;

namespace NewsPlus.API.Controllers
{
    [Route("api/config")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly IConfigRepository _repo;

        public ConfigController(IConfigRepository repo)
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
        public async Task<ActionResult> Get(Guid id)
        {

            var item = await _repo.GetById(id);

            if (item == null)
            {
                return NotFound(new ApiNotFoundResponse($"Config with id: {id} is not found"));
            }

            return Ok(item);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Post(CreateConfigViewModel model)
        {
            var result = await _repo.Create(model);

            if (result.Result > 0)
            {
                return RedirectToAction(nameof(Get), new { id = result.Id });
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Create config failed"));
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Put(UpdateConfigViewModel model, Guid id)
        {
            var item = await _repo.GetById(id);
            if (item == null)
                return NotFound(new ApiNotFoundResponse($"Config with id: {id} is not found"));

            var result = await _repo.Update(id, model);

            if (result.Result > 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Update config failed"));
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = _repo.GetById(id);

            if (item == null)
                return NotFound(new ApiNotFoundResponse($"Config with id: {id} is not found"));

            var result = await _repo.Delete(id);

            if (result > 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Delete config failed"));
            }

        }
    }
}
