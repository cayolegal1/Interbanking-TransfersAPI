using API.Model;
using Microsoft.AspNetCore.Mvc;
using API.Data.Repositories.Clients;
using FluentValidation;

namespace PostgresWebAPI.Controllers
{
    //decoradores
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : Controller
    {
        private readonly IClientRepository _clientRepository;

        ClientValidator validator = new ClientValidator();
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

                var response = await _clientRepository.getClientbyCedula(cedula);

                if (response == null)
                {
                    throw new Exception("Cliente no existe");
                }
                return Ok(response);


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

            
            Client clientIsValid = new Client()
            {
                cedula = clientParam.cedula,
                tipo_doc = clientParam.tipo_doc,
                nombre_apellido = clientParam.nombre_apellido
            };



            var clientValidator = validator.Validate(clientIsValid);

            if (!clientValidator.IsValid)
            {
                foreach (var error in clientValidator.Errors)
                {
                    return BadRequest($"Error en el servicio: {error.ErrorMessage}. Campo: {error.PropertyName}");
                }
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

            Client clientIsValid = new Client()
            {
                cedula = clientParam.cedula,
                tipo_doc = clientParam.tipo_doc,
                nombre_apellido = clientParam.nombre_apellido
            };

            var clientValidator = validator.Validate(clientIsValid);

            if (!clientValidator.IsValid)
            {
                foreach (var error in clientValidator.Errors)
                {
                    return BadRequest($"Error en el servicio: {error.ErrorMessage}. Campo: {error.PropertyName}");
                }
            }

            try
            {
                var newClient = await _clientRepository.updateClient(clientParam);
                return Created("Client information up to date", clientParam);

            } catch (Exception ex) 

            {
                return BadRequest($"Error en el servicio: {ex.Message}"); 
            }

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