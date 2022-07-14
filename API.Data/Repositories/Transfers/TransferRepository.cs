using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Model;
using Dapper;
using Npgsql;
using API.Data.Repositories.Transfers;
using API.Data.Repositories.Accounts;

namespace API.Data.Repositories.Transfers
{
    public class TransferRepository : ITransferRepository
    {
        protected NpgsqlConnection dbConnection()
        {
            return new NpgsqlConnection("Server = 127.0.0.1; Port = 5432; Database = interbanking_transfers;" +
            " User Id = postgres; Password = hola1606;");

        }


        public async Task<double> HasEnoughMoney(string id_cta)
        {

            var db = dbConnection();

            string query = @$"


            SELECT saldo FROM cuentas WHERE id_cta = @id_cta

            ";

            var response = await db.QueryFirstOrDefaultAsync<Account>(query, new { id_cta });

            return response.saldo;

        }

        public async Task<bool> MakeTransfer(Transfer accountInfo)
        {

            var db = dbConnection();
            //Account account = accountInfo.
            string id_cta = accountInfo.num_cta;

            double enoughMoney = await HasEnoughMoney(id_cta);

            if (enoughMoney < accountInfo.monto)
            {
                throw new Exception("Saldo insuficiente");
            }

            if (accountInfo.cod_banco_origen == accountInfo.cod_banco_destino)
            {
                throw new Exception("Para realizar una transferencia deben ser bancos diferentes");
            }

            string query = @"

             INSERT INTO transferencias
             (id_transaccion, num_cta, cedula_cliente, fecha, monto, estado, cod_banco_origen, cod_banco_destino)
             VALUES(@id_transaccion, @num_cta, @cedula_cliente, @fecha, @monto, @estado, @cod_banco_origen, @cod_banco_destino)
            ";


            var response = await db.ExecuteAsync(query,
            new
            {
                accountInfo.id_transaccion,
                accountInfo.num_cta,
                accountInfo.cedula_cliente,
                accountInfo.fecha,
                accountInfo.monto,
                accountInfo.estado,
                accountInfo.cod_banco_origen,
                accountInfo.cod_banco_destino
            });
            return response > 0;
        }

        public async Task<Transfer> GetTransferDataByID(string id_transaccion)
        {

            var db = dbConnection();
            string query = "SELECT * FROM transferencias WHERE id_transaccion = @id_transaccion";
            var response = await db.QueryFirstOrDefaultAsync<Transfer>(query, new { });
            return response;

        }

        public async Task<Transfer> ChangeTransferState(string code)
        {


            throw new NotImplementedException();
        }

        public Task<IEnumerable<Transfer>> GetAllTransfersData()
        {
            throw new NotImplementedException();
        }

    }
}
