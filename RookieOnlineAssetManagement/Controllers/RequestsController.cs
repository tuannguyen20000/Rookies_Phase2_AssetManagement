using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RookieOnlineAssetManagement.Service.IServices;
using System;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.Controllers
{
  [Produces("application/json")]
  [Route("api/[controller]")]
  [ApiController]
  public class RequestsController : ControllerBase
  {
    private readonly IRequestService _requestService;

    public RequestsController(IRequestService requestService)
    {
      _requestService = requestService;
    }

    [HttpPut]
    [Route("complete-request/{id}")]
    public async Task<IActionResult> CompleteRequest(int id)
    {
      var result = await _requestService.CompleteRequestAsync(id);
      if (result == null) return BadRequest();
      return Ok(result);
    }

    [HttpPut]
    [Route("cancel-request/{id}")]
    public async Task<IActionResult> CancelRequest(int id)
    {
      var result = await _requestService.CancelRequestAsync(id);
      if (result == null) return BadRequest();
      return Ok(result);
    }

    [HttpPut]
    [Route("create-request/{id}")]
    public async Task<IActionResult> CreateRequest(int id)
    {
      var request = await _requestService.CreateRequestAsync(id);
      if (request == null) return BadRequest();
      return Ok(request);
    }

    [HttpGet]
    [Route("get-requests")]
    public async Task<IActionResult> GetRequestList(int? page, int? pageSize, string keyword, DateTime returnedDate, [FromQuery] string[] states, string sortOrder, string sortField)
    {
      var requestList = await _requestService.GetRequestListAsync(page, pageSize, keyword, returnedDate, states, sortOrder, sortField);
      if (requestList == null) return BadRequest();
      return Ok(requestList);
    }
  }
}
