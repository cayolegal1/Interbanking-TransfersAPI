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

        [HttpGet]
        public async Task<IActionResult> getAllTransfers()
        {
            var request = await _transferRepository.GetAllTransfersData();

            return Ok(request);
        }

        [HttpGet("{num_cta}/enviados")]
        public async Task<IActionResult>getSendedTransfersDataByAccountNumber(string num_cta)
        {
            var request = await _transferRepository.GetSendedTransfersData(num_cta);

            return Ok(request);
        }
        [HttpGet("{num_cta}/recibidos")]
        public async Task<IActionResult>getReceivedTransfersDataByAccountNumber(string num_cta)
        {
            var request = await _transferRepository.GetReceivedTransfersData(num_cta);

            return Ok(request);
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


        [HttpPatch]
        public async Task<IActionResult> changeTransferState([FromBody] Transfer transferInfo)
        {   
            try
            {
            var request = await _transferRepository.updateTransferState(transferInfo);

            return Created("Done", transferInfo);

            } catch(Exception ex)
            {
                return BadRequest($"Error en el servicio. {ex.Message}.\n{ex.StackTrace}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getTransferDataByID(string id)
        {   
            try
            {
            var request = await _transferRepository.GetTransferDataByID(id);

            return Ok(request);

            } catch(Exception ex)
            {
                return BadRequest($"Error en el servicio: {ex.Message}.\n{ex.StackTrace}");
                
            }
        }

        [HttpDelete("{id_transaccion}")]
        public async Task<IActionResult> deleteTransfer(string id_transaccion)
        {
            var requestDelete = await _transferRepository.deleteTransferByID(id_transaccion);
            return Ok(requestDelete);
        }
        

    }
}
