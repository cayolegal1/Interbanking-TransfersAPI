using Microsoft.AspNetCore.Mvc;
using API.Data.Repositories.Accounts;
using API.Model;
using FluentValidation;

namespace PostgreSQLProjectAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        AccountValidator validator = new AccountValidator();
        public AccountsController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            return Ok(await _accountRepository.getAllAccounts());
        }



        [HttpGet("{id_cta}")]
        public async Task<IActionResult> GetAccountByID(string id_cta)
        {   
            var request = await _accountRepository.getAccountByID(id_cta);

            if(request == null)
            {
                return BadRequest("Cliente no existe");
            }

            try
            {
            return Ok(await _accountRepository.getAccountByID(id_cta));

            } catch (Exception ex)
            {

            return BadRequest("Error en el servicio: " + ex.Message + "\n" + ex.StackTrace);

            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] Account accountInfo)
        {

            if (accountInfo == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guid myGuid = Guid.NewGuid();

            var newAccount = new Account()
            {
                id_cta = myGuid.ToString(),
                num_cta = accountInfo.num_cta,
                moneda = accountInfo.moneda,
                saldo = accountInfo.saldo,
                cedula_cliente = accountInfo.cedula_cliente,
                cod_banco = accountInfo.cod_banco,
            };

            var createValidator = validator.Validate(newAccount);

            if (!createValidator.IsValid)
            {
                foreach (var error in createValidator.Errors)
                {
                    return BadRequest($"Error en el servicio: {error.ErrorMessage}.");
                }
            }
       

            try
            {

            var newClient = await _accountRepository.createAccount(newAccount);

            return Created("Client created", accountInfo);

            } catch(Exception ex)
            {
                return BadRequest("Error en el servicio: " + ex.Message + "\n" + ex.StackTrace);
            }
        }


        [HttpPut]
        public async Task<IActionResult> UpdateAccountInfo([FromBody] Account accountInfo)
        {
  
            try
            {

            if (accountInfo == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var request = validator.Validate(accountInfo);

            if (!request.IsValid)
            {
                foreach (var error in request.Errors)
                {
                    return BadRequest($"Error en el servicio: {error.ErrorMessage}.");
                }
            }


            var newClient = await _accountRepository.updateAccount(accountInfo);

            return Created("Client information up to date", accountInfo);

            } catch(Exception ex)
            {
                return BadRequest($"Error en el servicio: {ex.Message}.\n{ex.StackTrace}.");
            }
        }

        [HttpDelete("{id_cta}")]
        public async Task<IActionResult> DeleteAccount(string id_cta)
        {
            try
            {
            if (id_cta == null)
            {
                return BadRequest();
            }

            var newClient = await _accountRepository.deleteAccount(new Account { id_cta = id_cta });

            return NoContent();

            } catch(Exception ex)
            {
                return BadRequest($"Error en el servicio: {ex.Message}.\n{ex.StackTrace}.");
            }
        }
    }
}
