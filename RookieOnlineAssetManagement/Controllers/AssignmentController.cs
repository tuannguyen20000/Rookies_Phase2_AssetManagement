using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RookieOnlineAssetManagement.Entities.Dtos.AssignmentService;
using RookieOnlineAssetManagement.Service.IServices;
using System;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;
        public AssignmentController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [HttpPost]
        [Route("create-assignment")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAssignment(CreateAssignmentDto request)
        {
            var result = await _assignmentService.CreateAssignmentAsync(request);
            return Ok(result);
        }

        [HttpGet("[action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DetailsAssignment(int id)
        {
            var assignment = await _assignmentService.DetailsAssignmentAsync(id);
            return Ok(assignment);
        }


        [HttpPut("[action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAssignment(int id, UpdateAssignmentDto assignmentDto)
        {
            var assignmentUpdated = await _assignmentService.UpdateAssignmentAsync(id, assignmentDto);
            if (assignmentUpdated == null) return BadRequest();
            return Ok(assignmentUpdated);
        }

        [HttpDelete("[action]/{id}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            var assignmentUpdated = await _assignmentService.DisableAssignmentAsync(id);
            if (assignmentUpdated == null) return BadRequest();
            return Ok(assignmentUpdated);
        }

        [HttpGet]
        [Route("detail-assignment/{Id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetDetail(int Id)
        {
            var data = await _assignmentService.GetDetailsAssignmentByIdAsync(Id);
            if (data == null) return BadRequest(data);
            return Ok(data);
        }

        [HttpGet]
        [Route("getlist-assignment")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetList(int? page, int? pageSize, string keyword, DateTime assignedDate, [FromQuery] string[] states, string sortOrder, string sortField)
        {
            var assignments = await _assignmentService.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            if (assignments == null) return BadRequest(assignments);
            return Ok(assignments);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Staff")]
        [Route("getuser-assignment/{userId}/{sortOrder}/{sortField}")]
        public async Task<IActionResult> GetUserAssignment(string userId, string sortOrder, string sortField)
        {
            var assignments = await _assignmentService.GetAssignmentByUserIdAsync(userId, sortOrder, sortField);
            if (assignments == null) return BadRequest(assignments);
            return Ok(assignments);
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Staff")]
        [Route("accept/{id}")]
        public async Task<IActionResult> AcceptAssignment(int id)
        {
            var result = await _assignmentService.Accepted(id);
            if (result == false) return BadRequest(result);
            return Ok(result);
        }
    }
}
