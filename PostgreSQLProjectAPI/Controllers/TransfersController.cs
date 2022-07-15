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

            Guid guid = Guid.NewGuid();

            var newTransfer = new Transfer()
            {
                id_transaccion = guid.ToString(),
                num_cta = transferInfo.num_cta,
                cedula_cliente = transferInfo.cedula_cliente,
                fecha = transferInfo.fecha,
                monto = transferInfo.monto,
                estado = transferInfo.estado,
                cod_banco_origen = transferInfo.cod_banco_origen,   
                cod_banco_destino = transferInfo.cod_banco_destino,
                num_cta_destino = transferInfo.num_cta_destino,

            };

            var newClient = await _transferRepository.MakeTransfer(newTransfer);

            return Created("Done", transferInfo);
        }
    }
}
