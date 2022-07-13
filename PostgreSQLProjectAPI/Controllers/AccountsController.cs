using Microsoft.AspNetCore.Mvc;
using API.Data.Repositories.Accounts;
using API.Model;

namespace PostgreSQLProjectAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : Controller
    {
        private readonly IAccountRepository _accountRepository;
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
            return Ok(await _accountRepository.getAccountByID(id_cta));
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

            var newClient = await _accountRepository.createAccount(newAccount);

            return Created("Client created", accountInfo);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateAccountInfo([FromBody] Account accountInfo)
        {

            if (accountInfo == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newClient = await _accountRepository.updateAccount(accountInfo);

            return Created("Client information up to date", accountInfo);
        }

        [HttpDelete("{id_cta}")]
        public async Task<IActionResult> DeleteAccount(string id_cta)
        {

            if (id_cta == null)
            {
                return BadRequest();
            }

            var newClient = await _accountRepository.deleteAccount(new Account { id_cta = id_cta });

            return NoContent();
        }
    }
}
