using Microsoft.AspNetCore.Mvc;
using API.Model;
using API.Data.Repositories.Banks;
using FluentValidation;

namespace PostgreSQLProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanksController : Controller
    {   
        private readonly IBankRepository _bankRepository;
        public BanksController(IBankRepository bankRepository)
        {
            _bankRepository = bankRepository;
        }

        BankValidator bankValidator = new BankValidator();

        [HttpGet]
        public async Task<IActionResult> GetAllBanks()
        {
            return Ok(await _bankRepository.getAllBanks());
        }



        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetBankbyCode(string codigo)
        {
            var request = await _bankRepository.getBankbyCode(codigo);

            if (request == null)
            {
                throw new Exception("Banco no existe");
            }

            return Ok(await _bankRepository.getBankbyCode(codigo));
        }




        [HttpPost]
        public async Task<IActionResult> CreateBank([FromBody] Bank bankInfo)
        {

            if (bankInfo == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bankValidation = new Bank()
            {
                codigo_banco = bankInfo.codigo_banco,
                nombre_banco = bankInfo.nombre_banco,
                direccion = bankInfo.direccion
            };

            var validator = bankValidator.Validate(bankValidation);

            if(!validator.IsValid)
            {
               foreach(var error in validator.Errors)
               {   
                 return BadRequest($"Error en el servicio: {error.ErrorMessage}.");
               }
            }

            try
            {
                var newClient = await _bankRepository.createBank(bankInfo);

                return Created("Client created", bankInfo);

            }catch(Exception ex)
            {
                return BadRequest($"{ex.Message}.\n {ex.StackTrace}");
            }

        }


        [HttpPut]
        public async Task<IActionResult> UpdateBankInfo([FromBody] Bank bankInfo)
        {

            if (bankInfo == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bankValidation = new Bank()
            {
                codigo_banco = bankInfo.codigo_banco,
                nombre_banco = bankInfo.nombre_banco,
                direccion = bankInfo.direccion
            };

            var validator = bankValidator.Validate(bankValidation);

            if (!validator.IsValid)
            {
                foreach (var error in validator.Errors)
                {
                    return BadRequest($"Error en el servicio: {error.ErrorMessage}.");
                }
            }

            var newClient = await _bankRepository.updateBank(bankInfo);

            return Created("Client information up to date", bankInfo);
        }

        [HttpDelete("{codigo_banco}")]
        public async Task<IActionResult> DeleteBank(string codigo_banco)
        {

            if (codigo_banco == null)
            {
                return BadRequest();
            }

            var newClient = await _bankRepository.deleteBank(new Bank { codigo_banco = codigo_banco });

            return NoContent();
        }
    }
}


