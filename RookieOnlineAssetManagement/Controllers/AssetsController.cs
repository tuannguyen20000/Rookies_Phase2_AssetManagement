using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using RookieOnlineAssetManagement.Entities.Dtos.AssetService;
using RookieOnlineAssetManagement.Service.IServices;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetService _assetService;

        public AssetsController(IAssetService assetService)
        {
            _assetService = assetService;
        }
        [HttpGet]
        [Route("get-all-categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _assetService.GetCategoriesAsync();
            if (categories == null) return BadRequest(categories);
            return Ok(categories);
        }

        [HttpGet]
        [Route("getlist")]
        public async Task<IActionResult> GetList(int? page, int? pageSize, string keyword, string sortOrder, string sortField)
        {
            var result = await _assetService.GetAssetListAvailableAsync(page, pageSize, keyword, sortOrder, sortField);
            if (result == null) return BadRequest(result);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> GenerateAssetCode(CreateAssetDto createAsset)
        {
            var assetCode = await _assetService.CreateAssetAsync(createAsset);
            return Ok(assetCode);
        }

        [HttpGet]
        [Route("get-update-detail/{assetId}")]
        public async Task<IActionResult> GetUpdateDetail(int assetId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var asset = await _assetService.GetUpdateAsync(assetId);
            if (asset == null) return BadRequest(asset);
            return Ok(asset);
        }

        [HttpPut]
        [Route("update/{assetId}")]
        public async Task<IActionResult> UpdateAsset(int assetId, UpdateAssetDto assetDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var asset = await _assetService.UpdateAsync(assetId, assetDto);
            if (asset == null) return BadRequest(asset);
            return Ok(asset);
        }

        [HttpGet]
        [Route("asset-list")]
        public async Task<IActionResult> GetAssetsListAsync(int? page, int? pageSize, string keyword, [FromQuery] string[] states, [FromQuery] string[] categories, string sortOrder, string sortField)
        {
            var list = await _assetService.GetAllAssetsAsync(page, pageSize, keyword, states, categories, sortOrder, sortField);
            return Ok(list);
        }

        [HttpPost]
        [Route("create-category")]
        public async Task<IActionResult> CreateCategory(CategoryDto model)
        {
            var category = await _assetService.CreateCategoryAsync(model);
            return Ok(category);
        }

        [HttpPost]
        [Route("create-asset")]
        public async Task<IActionResult> CreateAsset(CreateAssetDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var asset = await _assetService.CreateAssetAsync(model);
            return Ok(asset);
        }

        [HttpGet]
        [Route("{assetId}")]
        public async Task<IActionResult> GetAssetsDetailAsync(int assetId)
        {
            var asset = await _assetService.GetAssetDetailsAsync(assetId);
            return Ok(asset);
        }

        [HttpGet]
        [Route("get-state/{assetId}")]
        public async Task<IActionResult> GetState(int assetId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var state = await _assetService.GetStateAsync(assetId);
            if (state == null) return BadRequest(state);
            return Ok(state);
        }

        [HttpPut]
        [Route("delete/{assetId}")]
        public async Task<IActionResult> Delete(int assetId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _assetService.DeleteAsync(assetId);
            if (result == false) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet]
        [Route("history/{assetId}")]
        public async Task<IActionResult> GetAssetHistoryAsync(int assetId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var history = await _assetService.GetHistoryAsync(assetId);
            if (history == null) return BadRequest(history);
            return Ok(history);
        }

        [HttpGet]
        [Route("checkHistory/{assetId}")]
        public async Task<IActionResult> CheckHistory(int assetId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _assetService.CheckHistoryAssetAsync(assetId);
            return Ok(result);
        }
    }
}
