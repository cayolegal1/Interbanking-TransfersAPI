using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using API.Data.Repositories.Banks;
using API.Model;

namespace API.Data.Repositories.Banks
{
    public class BankValidator: AbstractValidator<Bank>
    {
        public BankValidator()
        {
            RuleFor(bank => bank.codigo_banco).NotNull().Length(8).NotEmpty();
            RuleFor(bank => bank.nombre_banco).NotNull().NotEmpty().MaximumLength(40).MinimumLength(5);
            RuleFor(bank => bank.direccion).NotNull().NotEmpty().MaximumLength(50);
        }
    }
}
