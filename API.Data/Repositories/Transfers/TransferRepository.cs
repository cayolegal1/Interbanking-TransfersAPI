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
using API.Data.Repositories.Banks;

namespace API.Data.Repositories.Transfers
{
    public class TransferRepository : ITransferRepository
    {


        protected NpgsqlConnection dbConnection()
        {
            return new NpgsqlConnection("Server = 127.0.0.1; Port = 5432; Database = interbanking_transfers;" +
            " User Id = postgres; Password = hola1606;");

        }


        public async Task<double> HasEnoughMoney(string num_cta)
        {

            var db = dbConnection();

            string query = @$"


            SELECT saldo FROM cuentas WHERE num_cta = @num_cta

            ";

            var response = await db.QueryFirstOrDefaultAsync<Account>(query, new { num_cta });

            return response.saldo;

        }

        public async Task<bool> MakeTransfer(Transfer accountInfo)
        {

            var db = dbConnection();

            var account = new AccountRepository();

            var bank = new BankRepository();

            var accountOrigin = await account.searchAccountByNumber(accountInfo.num_cta);

            var accountDestiny = await account.searchAccountByNumber(accountInfo.num_cta_destino);

            var bankOrigin = await bank.getBankbyCode(accountInfo.cod_banco_origen);

            var bankDestiny = await bank.getBankbyCode(accountInfo.cod_banco_destino);

            double enoughMoney = await HasEnoughMoney(accountInfo.num_cta);


            if(accountOrigin == null)
            {
                throw new Exception("La cuenta que realizará la transacción no existe");
            }

            if(accountDestiny == null)
            {
                throw new Exception("La cuenta que recibirá el dinero no existe");
            }

            if (enoughMoney < accountInfo.monto)
            {
                throw new Exception("Saldo insuficiente");
            }

            if (accountInfo.cod_banco_origen == accountInfo.cod_banco_destino)
            {
                throw new Exception("Para realizar una transferencia deben ser bancos diferentes");
            }

            //if(bankOrigin == null) 
            //{
            //    throw new Exception("Banco origen no existe");
            //}

            //if(bankDestiny == null)
            //{
            //    throw new Exception("Banco destino no existe");
            //}


            string query = @"

             INSERT INTO transferencias
             (id_transaccion, num_cta, num_cta_destino, cedula_cliente, fecha, monto, estado, cod_banco_origen, cod_banco_destino)
             VALUES(@id_transaccion, @num_cta, @num_cta_destino, @cedula_cliente, @fecha, @monto, @estado, @cod_banco_origen, @cod_banco_destino)";


            var response = await db.ExecuteAsync(query,
            new
            {
                accountInfo.id_transaccion,
                accountInfo.num_cta,
                accountInfo.num_cta_destino,
                accountInfo.cedula_cliente,
                accountInfo.fecha,
                accountInfo.monto,
                accountInfo.estado,
                accountInfo.cod_banco_origen,
                accountInfo.cod_banco_destino
            });


            if(response > 0)
            {
                string originAccountMoneyUpdateQuery =@"UPDATE cuentas SET saldo = saldo - @monto WHERE num_cta = @num_cta";

                await db.ExecuteAsync(originAccountMoneyUpdateQuery, new { accountInfo.monto, accountInfo.num_cta } );

                string destinyAccountMoneyUpdateQuery = @"UPDATE cuentas SET saldo = saldo + @monto WHERE num_cta = @num_cta_destino";

                await db.ExecuteAsync(destinyAccountMoneyUpdateQuery, new { accountInfo.monto, accountInfo.num_cta_destino });
            }
            return response > 0;    
        }



        public async Task<bool> updateTransferState(Transfer data)
        {

            var db = dbConnection();

            string query = @"UPDATE transferencias SET estado = @estado WHERE id_transaccion = @id_transaccion";

            var idTransfer = GetTransferDataByID(data.id_transaccion);

            if(idTransfer == null)
            {
                throw new Exception("El id no existe"); 
            }

            var response = await db.ExecuteAsync(query, new {data.estado, data.id_transaccion});

            return response > 0;

        }

        public async Task<IEnumerable<Transfer>> GetAllTransfersData()
        {
            var db = dbConnection();

            string query = @"SELECT * FROM transferencias";

            var response = await db.QueryAsync<Transfer>(query);

            return response;
        }

        public async Task<bool> deleteTransferByID(string id_transaccion)
        {
            var db = dbConnection();

            string query = @"DELETE FROM transferencias WHERE id_transaccion = @id_transaccion";

            var idToDelete = await GetTransferDataByID(id_transaccion);

            if(idToDelete == null)
            {
                throw new Exception("El ID de la operación no es válido");
            }

            var response = await db.ExecuteAsync(query, new { id_transaccion });

            return response > 0;
        }

        public async Task<Transfer> GetTransferDataByID(string id_transaccion)
        {

            var db = dbConnection();
            string query = "SELECT * FROM transferencias WHERE id_transaccion = @id_transaccion";
            var response = await db.QueryFirstOrDefaultAsync<Transfer>(query, new {id_transaccion});
            return response;

        }
        public async Task<IEnumerable<Transfer>>GetSendedTransfersData(string num_cta)
        {
            var db = dbConnection();
            string query = @"SELECT * FROM transferencias WHERE num_cta = @num_cta";

            var response = await db.QueryAsync<Transfer>(query, new {num_cta});

            return response;
        }

        public async Task<IEnumerable<Transfer>> GetReceivedTransfersData(string num_cta_destino)
        {

            var db = dbConnection();

            string query = @"SELECT * FROM transferencias WHERE num_cta_destino = @num_cta_destino";

            var response = await db.QueryAsync<Transfer>(query, new {num_cta_destino});

            return response;
        }

    }
}
