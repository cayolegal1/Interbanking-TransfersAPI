using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using API.Model;

namespace API.Data.Repositories.Banks
{
    public class BankRepository : IBankRepository
    {

        protected NpgsqlConnection dbConnection()
        {
            return new NpgsqlConnection("Server = 127.0.0.1; Port = 5432; Database = interbanking_transfers; User Id = postgres; Password = hola1606;");
        }
        public async Task<bool> createBank(Bank bankInfo)
        {
            var db = dbConnection();

            string query = @$"
             
             INSERT INTO bancos (codigo_banco, nombre_banco, direccion)
             VALUES(codigo_banco, @nombre_banco, @direccion)
             
             ";

            var response = await db.ExecuteAsync(query, 
            new { bankInfo.nombre_banco, bankInfo.direccion, bankInfo.codigo_banco });

            return response > 0;
        }

        public async Task<bool> deleteBank(Bank bankInfo)
        {
            var db = dbConnection();

            string query = @"
                
             DELETE FROM bancos 
             WHERE codigo_banco = @codigo_banco

            ";

            var response = await db.ExecuteAsync(query, new {bankInfo.codigo_banco});

            return response > 0;
        }

        public async Task<IEnumerable<Bank>> getAllBanks()
        {
            var db = dbConnection();

            string query = @"
            
             SELECT * FROM bancos

            ";

            return await db.QueryAsync<Bank>(query);
        }

        public async Task<Bank> getBankbyCode(string codigo_banco)
        {
            var db = dbConnection();

            var query = @$"
            
             SELECT * FROM bancos WHERE codigo_banco = @codigo_banco
            
            ";

            var response = await db.QueryFirstOrDefaultAsync<Bank>(query, new {codigo_banco});

            return response;
        }

        public async Task<bool> updateBank(Bank bankInfo)
        {
            var db = dbConnection();

            string query = @$"
             
             UPDATE bancos
             SET nombre_banco = @nombre_banco,
             direccion = @direccion
             WHERE codigo_banco = @codigo_banco
             ";

            var response = await db.ExecuteAsync(query,
            new { bankInfo.nombre_banco, bankInfo.direccion, bankInfo.codigo_banco });

            return response > 0;
        }
    }
}
