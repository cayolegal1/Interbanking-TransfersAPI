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
            try
            {
            return Ok(await _clientRepository.getClientbyCedula(cedula));
            } catch (Exception ex)
            {
                return BadRequest($"Error en el servicio: {ex.Message}.\n{ex.StackTrace}.\n{ex.GetType()}");  
            }
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

            try
            {

            var newClient = await _clientRepository.createClient(clientParam);

            return Created("Client created", clientParam);

            } catch(Exception ex)
            {
                return BadRequest($"Error en la creación de cliente: {ex.Message}.\n{ex.StackTrace}");
            }

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
            try
            {
            if (cedula == null)
            {
                return BadRequest();
            }

            var newClient = await _clientRepository.deleteClient(new Client { cedula = cedula });

            return NoContent();

            } catch(Exception ex)
            {
                return BadRequest($"Error en el servicio: {ex.Message}.\n{ex.StackTrace}");
            }

        }
    }
}