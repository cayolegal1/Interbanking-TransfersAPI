using API.Model;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Data.Repositories.Clients
{
    public class ClientRepository : IClientRepository

    {

        //creamos una variable global de la clase para que podamos esperar un valor que nos llegue 
        //del método constructor cuando creemos una instancia de ella
        private PostgreSQLConfiguration _connection;


        //método que recibe como parámetro una conexión y se le asigna a la variable global     
        public ClientRepository(PostgreSQLConfiguration connectionString)
        {
            _connection = connectionString;
        }

        //Estamos instanciando el driver que nos sirve para conectarnos a postgres, pasandole como argumento 
        //en el constructor(seguramente recibe la conexión directamente al ser instanciada) nuestra variable 
        //global que será asignada cuando instanciemos la clase, y usando el método ConnectionString de NpgsqlConnection.
        //En dicho método estará el ConnectionString que definimos en Program en el método Connect de PostgreSqlConfiguration.
        //En donde este tiene acceso y usa la configuración o las variables de conexión a la base de datos
        protected NpgsqlConnection dbConnection()
        {
            return new NpgsqlConnection("Server = 127.0.0.1; Port = 5432; Database = interbanking_transfers; User Id = postgres; Password = hola1606;");
        }

        public async Task<IEnumerable<Client>> getAllClients()
        {
            //usamos el método que generamos para conectarnos a la base de datos
            var db = dbConnection();

            //generamos la consulta. El @ antes de las comillas nos sirve para hacer templates strings
            string query = @"
            
             SELECT * FROM clientes
            
            ";

            //Especificamos que devolverá un objeto de tipo cliente, es decir, los registros de nuestra tabla. Pero al decir 
            //que queremos que devuelva de tipo cliente nos estamos refiriendo a la clase en si, la clase esta para traer 
            //la data de una entidad que se le asigne
            return await db.QueryAsync<Client>(query, new { });
        }

        public async Task<bool> createClient(Client clientParam)
        {   
         

            var db = dbConnection();

            string query = @$"
             
             INSERT INTO clientes (cedula, tipo_doc, nombre_apellido)
             VALUES(@cedula, @tipo_doc, @nombre_apellido)
             ";

            var filterClient = await getClientbyCedula(clientParam.cedula);

            if(filterClient != null)
            {
                throw new Exception("Cliente ya se encuentra registrado");

            }

            //para hacer peticiones de tipo POST nos viene bien usar ExecuteAsync. Esto devuelve un int, si todo salió bien 
            //debemos revolver: response > 0. Ya que, devolverá 1 si se ejecuto correctamente la query o si al menos una 
            //fila fue afectada
            var response = await db.ExecuteAsync(query, new { clientParam.cedula, clientParam.tipo_doc, clientParam.nombre_apellido, 
            });

            return response > 0;

        }

        public async Task<bool> deleteClient(Client clientParam)
        {
            var db = dbConnection();

            string query = @$"
             
             DELETE FROM clientes
             WHERE cedula = @cedula
             ";

            var clientToDelete = await getClientbyCedula(clientParam.cedula);

            if(clientToDelete == null)
            {
                throw new Exception("Cliente no se encuentra registrado");
            }

            var response = await db.ExecuteAsync(query, new { clientParam.cedula });

            return response > 0;
        }


        public async Task<Client> getClientbyCedula(string cedula_cliente)
        {
            var db = dbConnection();

            var query = @$"
            
             SELECT * FROM clientes WHERE cedula = @cedula_cliente
            
            ";

            var response = await db.QueryFirstOrDefaultAsync<Client>(query, new {cedula_cliente});

            if(response == null)
            {
                throw new Exception("Cliente no existe");
            }

            return response;
        }

        public async Task<bool> updateClient(Client clientParam)
        {
            var db = dbConnection();

            string query = @$"
             
             UPDATE clientes
             SET tipo_doc = @tipo_doc,
             nombre_apellido = @nombre_apellido
             WHERE cedula = @cedula
             ";

            var response = await db.ExecuteAsync(query, new { clientParam.cedula, clientParam.tipo_doc, clientParam.nombre_apellido });

            return response > 0;
        }
    }
}
