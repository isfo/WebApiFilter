using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiFilter.Models
{
    /// <summary>
    /// Classe padrão de retorno para client-side
    /// </summary>
    public class Retorno
    {
        /// <summary>
        /// Preenchido com objeto retornado de determinado método
        /// </summary>
        public dynamic data;
        /// <summary>
        /// Caso tenha dado erro, este campo será true
        /// </summary>
        public bool hasError;
        /// <summary>
        /// Será preenchido automáticamente.
        /// </summary>
        public bool erroTratado = true;
        /// <summary>
        /// Lista com os erros.
        /// </summary>
        public Erros erros = new Erros();
        //List<Erro> erro = new List<Erro>();
        /// <summary>
        /// Mensagem de retorno
        /// </summary>
        public Mensagens msgs = new Mensagens();
        /// <summary>
        /// Converte o objeto numa string json
        /// </summary>
        /// <returns></returns>
        public JObject toReturn()
        {
            return JObject.FromObject(this, new JsonSerializer() { NullValueHandling = NullValueHandling.Ignore }); ;
        }
    }

    /// <summary>
    /// Classe de retorno para server-side
    /// </summary>
    public class RetornoInterno : Retorno
    {
        /// <summary>
        /// Type do retorno
        /// </summary>
        [JsonIgnore]
        public Type tipo;
    }

    /// <summary>
    /// Classe de mensagem do retorno
    /// </summary>
    public class Mensagem
    {
        public string code;
        public string msg;

        public Mensagem() { }
        public Mensagem(string code, string msg)
        {
            this.code = code;
            this.msg = msg;
        }
        public Mensagem(string msg)
        {
            this.msg = msg;
        }
    }

    /// <summary>
    /// Lista de <see cref="Mensagem"/>
    /// </summary>
    public class Mensagens : System.Collections.Generic.List<Mensagem>
    {
        private List<Mensagens> msgs = new List<Mensagens>();

        public void Add(string msg)
        {
            try
            {
                this.Add(null, msg);
            }
            catch
            {
            }
        }

        /// <summary>
        /// adiciona erro
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        public new void Add(string code, string msg)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                    this.Add(new Mensagem(msg));
                else
                    this.Add(new Mensagem(code, msg));
            }
            catch
            {
            }
        }

        /// <summary>
        /// adiciona erro
        /// </summary>
        /// <param name="msg"></param>
        public new void Add(Mensagem msg)
        {
            try
            {
                base.Add(msg);
            }
            catch
            {
            }
        }
    }

    /// <summary>
    /// Classe de erro para implementação na classe Retorno
    /// </summary>
    public class Erro
    {
        public string code { get; set; }
        public string msg { get; set; }

        public Erro() { }
        public Erro(string code, string msg)
        {
            this.code = code;
            this.msg = msg;
        }
        public Erro(string msg)
        {
            this.msg = msg;
        }
    }

    /// <summary>
    /// Classe de lista de <seealso cref="Erro"/>
    /// </summary>
    public class Erros : System.Collections.Generic.List<Erro>
    {
        private List<Erro> erros = new List<Erro>();

        public void Add(string msg)
        {
            try
            {
                this.Add(null, msg);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Adiciona Erro
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        public new void Add(string code, string msg)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                    this.Add(new Erro(msg));
                else
                    this.Add(new Erro(code, msg));
            }
            catch
            {
            }
        }

        /// <summary>
        /// Adiciona Erro
        /// </summary>
        /// <param name="erro"></param>
        public new void Add(Erro erro)
        {
            try
            {
                base.Add(erro);
            }
            catch
            {
            }
        }
    }
}