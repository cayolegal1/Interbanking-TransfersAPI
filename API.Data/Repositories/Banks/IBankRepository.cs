using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Model;

namespace API.Data.Repositories.Banks
{
    public interface IBankRepository
    {
        Task<IEnumerable<Bank>> getAllBanks();

        //En el caso de que querramos devolver solo un elemento no hace falta usar el método enumerable de task
        Task<Bank> getBankbyCode(string code);


        Task<bool> createBank(Bank bankInfo);

        Task<bool> updateBank(Bank bankInfo);

        Task<bool> deleteBank(Bank bankInfo);
    }
}
