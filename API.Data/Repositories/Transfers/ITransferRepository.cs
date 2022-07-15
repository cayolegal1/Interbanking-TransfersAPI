using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Model;

namespace API.Data.Repositories.Transfers
{
    public interface ITransferRepository
    { 
        Task<bool> MakeTransfer(Transfer accountInfo);

        Task<bool> updateTransferState(Transfer data);

        Task<bool> deleteTransferByID(string id_transaccion);


        Task<Transfer> GetTransferDataByID(string id_transaccion);

        Task<IEnumerable<Transfer>> GetAllTransfersData();

        Task<IEnumerable<Transfer>> GetSendedTransfersData(string num_cta);

        Task<IEnumerable<Transfer>> GetReceivedTransfersData(string num_cta_destino);



    }
}
