using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using API.Data.Repositories.Accounts;
using API.Model;

namespace API.Data.Repositories.Accounts
{
    public class AccountRepository : IAccountRepository
    {

        protected NpgsqlConnection dbConnection()
        {
            return new NpgsqlConnection("Server = 127.0.0.1; Port = 5432; Database = interbanking_transfers; User Id = postgres; Password = hola1606;");
        }


        public async Task<bool> createAccount(Account accountInfo)
        {

            var db = dbConnection();
            var query = @"
            INSERT INTO cuentas(id_cta, num_cta, moneda, saldo, cedula_cliente, cod_banco)
            VALUES(@id_cta, @num_cta, @moneda, @saldo, @cedula_cliente, @cod_banco)
             ";
            

            var response = await db.ExecuteAsync(query, new {accountInfo.id_cta, accountInfo.num_cta, accountInfo.moneda, accountInfo.saldo, accountInfo.cedula_cliente, accountInfo.cod_banco});

            return response > 0;
        }

        public async Task<bool> deleteAccount(Account accountInfo )
        {

            var db = dbConnection();
            var query = @"
            
            DELETE FROM cuentas 
            WHERE id_cta = @id_cta
            ";

            var response = await db.ExecuteAsync(query, new { accountInfo.id_cta });

            return response > 0;
     
        }

        public async Task<IEnumerable<Account>> getAllAccounts()
        {

            var db = dbConnection();
            string query = "SELECT * FROM cuentas";
            var response = await db.QueryAsync<Account>(query);
            return response;
        }


        public async Task<Account> getAccountByID(string id)
        {

            var db = dbConnection();
            string query = @"SELECT * FROM cuentas WHERE id_cta = @id";
            var response = await db.QueryFirstOrDefaultAsync<Account>(query, new {id});
            return response;
        }

        public async Task<bool> updateAccount(Account accountInfo)
        {

            var db = dbConnection();
            string query = @"
            UPDATE cuentas 
            SET num_cta = @num_cta,
            moneda = @moneda,
            saldo = @saldo
            WHERE id_cta = @id_cta";

            var response = await db.ExecuteAsync(
            query,
            new { accountInfo.num_cta, accountInfo.moneda, accountInfo.saldo, accountInfo.id_cta });

            return response > 0;
        }

    }
}
