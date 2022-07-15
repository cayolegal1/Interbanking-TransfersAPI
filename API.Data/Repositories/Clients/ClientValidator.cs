using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using API.Model;

namespace API.Data.Repositories.Clients
{
    public class ClientValidator : AbstractValidator<Client>
    {
        public ClientValidator()
        {
            RuleFor(client => client.cedula).NotNull().NotEmpty();


            string[] conditions = new string[]
            {
                "ci", "cedula", "cédula",
                "Ci", "Cedula", "Cédula",
                "CI", "CEDULA", "CÉDULA",

                "ruc", "RUC", "R.U.C.",

                "pasaporte", "Pasaporte", "PASAPORTE",
                "passport", "Passport", "PASSPORT"

            };

            RuleFor(client => client.tipo_doc).NotNull().NotEmpty().Must(value => conditions.Contains(value)).WithMessage("Tipo de documento inválido");

            RuleFor(client => client.nombre_apellido).NotNull().NotEmpty().MinimumLength(3).MaximumLength(30);

        }
    }
}
