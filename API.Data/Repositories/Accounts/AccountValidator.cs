using FluentValidation;
using API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Data.Repositories.Accounts
{
    public class AccountValidator: AbstractValidator<Account>
    {   
        public AccountValidator()
        {
            RuleFor(account => account.moneda).NotEmpty().NotNull().MaximumLength(3).MinimumLength(3);
            RuleFor(account => account.saldo).NotEmpty().NotNull();
            RuleFor(account => account.cedula_cliente).NotEmpty().NotNull();
            RuleFor(account => account.cod_banco).NotEmpty().NotNull().MaximumLength(8).MinimumLength(8);
            RuleFor(account => account.num_cta).NotEmpty().NotNull();
        }

    }
}
