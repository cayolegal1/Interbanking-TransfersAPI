using Microsoft.AspNetCore.Mvc;
using API.Data.Repositories.Transfers;
using API.Data.Repositories.Accounts;
using API.Model;

namespace PostgreSQLProjectAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TransfersController : Controller
    {
        private readonly ITransferRepository _transferRepository;

        public TransfersController(ITransferRepository transferRepository)
        {
            _transferRepository = transferRepository;
        }

        [HttpPost]
        public async Task<IActionResult> MakeTransfer([FromBody] Transfer transferInfo)
        {

            if (transferInfo == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newClient = await _transferRepository.MakeTransfer(transferInfo);

            return Created("Done", transferInfo);
        }
    }
}
