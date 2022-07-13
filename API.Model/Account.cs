using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Model
{
    public class Account
    {
        public string id_cta { get; set; }  

        public string num_cta { get; set; } 

        public string moneda { get; set; }

        public float saldo { get; set; }    

        public string cedula_cliente { get; set; }  

        public string cod_banco { get; set; }   
    }
}
