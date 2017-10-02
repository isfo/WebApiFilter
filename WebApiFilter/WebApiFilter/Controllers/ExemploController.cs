using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiFilter.Code;
using WebApiFilter.Models;

namespace WebApiFilter.Controllers
{
    public class ExemploController : ApiController
    {
        // GET: api/Exemplo
        public Retorno Get()
        {
            List<Usuario> lista = Comandos.GerarUsuarios();

            Filtro f = new Filtro(Request.GetQueryNameValuePairs());

            Retorno ret = f.AplicarFiltro<Usuario>(lista);

            return ret;
        }

        // GET: api/Exemplo/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Exemplo
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Exemplo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Exemplo/5
        public void Delete(int id)
        {
        }
    }
}
