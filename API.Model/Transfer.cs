using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Model
{
    public class Transfer
    {
        public string id_transaccion { get; set; }

        public string num_cta { get; set; } 

        public string num_cta_destino { get; set; }

        public string cedula_cliente { get; set; }

        public DateTime fecha { get; set; }

        public float monto { get; set; }

        public string estado { get; set; }

        public string cod_banco_origen { get; set; }

        public string cod_banco_destino { get; set; }
    }
}
