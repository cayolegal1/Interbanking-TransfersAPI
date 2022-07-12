using API.Model;
using Microsoft.AspNetCore.Mvc;
using API.Data.Repositories.Clients;

namespace PostgresWebAPI.Controllers
{
    //decoradores
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : Controller
    {
        private readonly IClientRepository _clientRepository;
        public ClientsController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClients()
        {
            return Ok(await _clientRepository.getAllClients());
        }

        [HttpGet("{cedula}")]
        public async Task<IActionResult> GetClientByCedula(string cedula)
        {
            return Ok(await _clientRepository.getClientbyCedula(cedula));
        }


        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] Client clientParam)
        {

            if (clientParam == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newClient = await _clientRepository.createClient(clientParam);

            return Created("Client created", clientParam);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateClient([FromBody] Client clientParam)
        {

            if (clientParam == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newClient = await _clientRepository.updateClient(clientParam);

            return Created("Client information up to date", clientParam);
        }

        [HttpDelete("{cedula}")]
        public async Task<IActionResult> DeleteClient(string cedula)
        {

            if (cedula == null)
            {
                return BadRequest();
            }

            var newClient = await _clientRepository.deleteClient(new Client { cedula = cedula });

            return NoContent();
        }
    }
}