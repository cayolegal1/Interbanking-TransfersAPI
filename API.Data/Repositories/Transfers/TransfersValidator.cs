using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using API.Model;

namespace API.Data.Repositories.Transfers
{
    public class TransfersValidator : AbstractValidator<Transfer>
    {
        public TransfersValidator()
        {

            RuleFor(model => model.cedula_cliente).NotNull().NotEmpty();
            RuleFor(model => model.num_cta).NotNull().NotEmpty();
            RuleFor(model => model.num_cta_destino).NotNull().NotEmpty();
            RuleFor(model => model.cod_banco_destino).NotNull().NotEmpty().Length(8);
            RuleFor(model => model.cod_banco_origen).NotNull().NotEmpty().Length(8);

            string[] conditions = new string[]
           {
                "ok", "pendiente", "rechazado"
           };
            RuleFor(model => model.estado).NotNull().NotEmpty().Must(x => conditions.Contains(x));
        }
    }
}