using Application.Interface;
using Application.Request.PurchasedPackage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasedPackageController : ControllerBase
    {
        private readonly IPurchasedPackageService _purchasedPackageService;

        public PurchasedPackageController(IPurchasedPackageService purchasedPackageService)
        {
            _purchasedPackageService = purchasedPackageService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPurchasedPackageAsync(PurchasedPackageRequest purchasedPackageRequest)
        {
            var response = await _purchasedPackageService.AddPurchasedPackageAsync(purchasedPackageRequest);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchasedPackageAsync()
        {
            var response = await _purchasedPackageService.GetPurchasedPackageAsync();
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        [HttpGet("{patientId}")]
        public async Task<IActionResult> GetPurchasedPackageByPatientIdAsync([FromRoute]string patientId)
        {
            var response = await _purchasedPackageService.GetPurchasedPackageByPatientIdAsync(patientId);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}
