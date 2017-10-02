using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiFilter.Models
{
    public class Usuario
    {
        public int id { get; set; }
        public string nome { get; set; }
        public DateTime nascimento { get; set; }
        public string cpf { get; set; }
        public int altura { get; set; }
        public string sexo { get; set; }

        public Usuario() { }

        public Usuario(int idParam, string nomeParam, DateTime nascParam, string cpfParam, int alturaParam, string sexoParam)
        {
            this.id = idParam;
            this.nome = nomeParam;
            this.nascimento = nascParam;
            this.cpf = cpfParam;
            this.altura = alturaParam;
            this.sexo = sexoParam;
        }

        
    }
}