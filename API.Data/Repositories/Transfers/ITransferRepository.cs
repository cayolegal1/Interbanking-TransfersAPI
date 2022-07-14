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

        Task<IEnumerable<Transfer>> GetAllTransfersData();

        Task<Transfer> GetTransferDataByID(string id_transaccion);

        Task<Transfer> ChangeTransferState(string code);
    }
}
