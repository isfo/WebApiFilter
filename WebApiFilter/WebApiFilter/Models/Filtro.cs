using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using WebApiFilter.Code;

namespace WebApiFilter.Models
{
    public class Filtro
    {
        /// <summary>
        /// Página que deseja receber, começa a partir do 1
        /// </summary>
        public int page = -1;
        /// <summary>
        /// O tamanho da página, caso seja página 2 com tamanho 20. Irá te retorna ((PAGE-1)*PAGESIZE)
        /// </summary>
        public int pageSize = -1;
        /// <summary>
        /// Lista de OrderBy
        /// </summary>
        public List<OrderBy> orderBy = new List<OrderBy>();

        public List<Wherer> where = new List<Wherer>();

        /// <summary>
        /// Gera um objeto Filtro a partir da query string da requisição
        /// <pre></pre>
        /// Propriedades PAGE, PAGESIZE, ORDERBY e ORDER.
        /// </summary>
        /// <param name="query">Lista das querystrings que compoem a requisição</param>
        public Filtro(IEnumerable<KeyValuePair<string, string>> query)
        {

            foreach (KeyValuePair<string, string> item in query)
            {
                switch (item.Key.ToString().ToLower())
                {
                    case "pagesize":
                        pageSize = Comandos.Converter.toInt(item.Value);
                        break;

                    case "page":
                        page = Comandos.Converter.toInt(item.Value);
                        break;

                    case "orderby":
                        string[] values = item.Value.Trim().Split(',');
                        foreach (string s in values)
                            orderBy.Add(new OrderBy(s.Trim().Split(' ')));
                        break;

                    case "where":
                        string[] valuesWhere = item.Value.Trim().Split(',');
                        foreach (string s in valuesWhere)
                            where.Add(new Wherer(s.Trim().Split(' ')));
                        break;
                }
            }
        }

        private Type RetornaTipoCampo(Type t, string campo)
        {
            PropertyInfo[] props = t.GetProperties();

            foreach (var prop in props)
            {
                if (prop.Name == campo)
                    return prop.PropertyType;
            }

            return t;
        }

        private bool VerificarCampoExists(Type t, string campo)
        {
            // verifico quais colunas fazem parte do tipo especificado ->
            PropertyInfo[] props = t.GetProperties();

            foreach (var prop in props)
            {
                if (prop.Name == campo)
                    return true;
            }

            return false;
        }

        public RetornoInterno AplicarWhere<T>(List<T> lista, int pos = 0, RetornoInterno ri = null)
        {
            if (ri == null)
                ri = new RetornoInterno();

            Type t = typeof(T);

            PropertyInfo[] props = t.GetProperties();

            if (this.where.Count() > pos)
            {
                Wherer where = this.where[pos];

                if (VerificarCampoExists(typeof(T), where.variavel))
                {
                    Type tipoDoCampo = RetornaTipoCampo(typeof(T), where.variavel);

                    try
                    {
                        var valor = Comandos.Converter.CastHelper.Cast(where.valor, tipoDoCampo);

                        if (where.valor == "NULL")
                            valor = null;

                        if (where.expressao == ">")
                            lista = lista.Where(c => (dynamic)c.GetType().GetProperties().First(m => m.Name == where.variavel).GetValue(c, null) > valor).ToList();
                        else if (where.expressao == "<")
                            lista = lista.Where(c => (dynamic)c.GetType().GetProperties().First(m => m.Name == where.variavel).GetValue(c, null) < valor).ToList();
                        else if (where.expressao == "==" || where.expressao == "=")
                            lista = lista.Where(c => (dynamic)c.GetType().GetProperties().First(m => m.Name == where.variavel).GetValue(c, null) == valor).ToList();
                        else if (where.expressao == "!=" || where.expressao == "=")
                            lista = lista.Where(c => (dynamic)c.GetType().GetProperties().First(m => m.Name == where.variavel).GetValue(c, null) != valor).ToList();
                        else if (where.expressao == ">=")
                            lista = lista.Where(c => (dynamic)c.GetType().GetProperties().First(m => m.Name == where.variavel).GetValue(c, null) >= valor).ToList();
                        else if (where.expressao == "<=")
                            lista = lista.Where(c => (dynamic)c.GetType().GetProperties().First(m => m.Name == where.variavel).GetValue(c, null) <= valor).ToList();
                        else if (where.expressao.ToLower() == "contains")
                            lista = lista.Where(c => (dynamic)c.GetType().GetProperties().First(m => m.Name == where.variavel).GetValue(c, null).ToString().ToLower().Contains(valor)).ToList();
                    }
                    catch (Exception ex)
                    {
                        ri.msgs.Add("[WHERE]Não foi possível converter o campo '" + where.variavel + "' para '" + tipoDoCampo.Name + "'.");
                    }

                    ri.data = AplicarWhere<T>(lista, pos + 1, ri).data;
                    ri.tipo = lista.GetType();

                    return ri;
                }
                else
                {
                    ri.msgs.Add("[WHERE]O campo '" + where.variavel + "' não está presente no retorno especificado.");
                }
            }


            ri.data = lista;
            ri.tipo = lista.GetType();

            return ri;
        }

        public IOrderedEnumerable<T> AplicarOrdenacao<T>(IOrderedEnumerable<T> oe, int pos = 0)
        {
            RetornoInterno ri = new RetornoInterno();
            Type t = typeof(T);

            if (this.orderBy.Count() > pos)
            {
                string sentido = this.orderBy[pos].sentido.ToLower();
                string coluna = this.orderBy[pos].coluna.ToLower();

                if (sentido == Enums.Ordem.ASC.ToLower())
                {
                    oe = oe.ThenBy(m => m.GetType()
                                  .GetProperties()
                                  .First(n => n.Name == coluna)
                                  .GetValue(m, null));
                }
                else
                {
                    oe = oe.ThenByDescending(m => m.GetType()
                                  .GetProperties()
                                  .First(n => n.Name == coluna)
                                  .GetValue(m, null));
                }

                ri.data = AplicarOrdenacao(oe, pos + 1);
                ri.tipo = typeof(RetornoInterno);

                return oe;
            }

            ri.data = oe;
            ri.tipo = typeof(IOrderedEnumerable<T>);

            return oe;
        }

        public RetornoInterno AplicarOrdenacao<T>(List<T> lista, int pos = 0, RetornoInterno ri = null)
        {
            if (ri == null)
                ri = new RetornoInterno();

            IOrderedEnumerable<T> oe = lista.OrderBy(c => c);
            Type t = typeof(T);

            // verifico quais colunas fazem parte do tipo especificado ->
            PropertyInfo[] props = t.GetProperties();

            //foreach (OrderBy item in this.orderBy)
            for (int i = 0; i < this.orderBy.Count; i++)
            {
                bool remove = true;
                foreach (var prop in props)
                {
                    if (prop.Name == orderBy[i].coluna)
                    {
                        remove = false;
                        continue;
                    }
                }
                if (remove)
                {
                    ri.msgs.Add("[ORDERBY]O campo '" + orderBy[i].coluna + "' não está presente no retorno especificado.");
                    this.orderBy.Remove(orderBy[i]);
                    i--;
                }
            }
            // <- termina a verificação

            if (this.orderBy.Count() > 0 + pos)
            {
                string sentido = this.orderBy[pos].sentido.ToLower();
                string coluna = this.orderBy[pos].coluna.ToLower();

                if (sentido == Enums.Ordem.ASC.ToLower())
                {
                    oe = lista.OrderBy(m => m.GetType()
                                  .GetProperties()
                                  .First(n => n.Name == coluna)
                                  .GetValue(m, null));
                }
                else
                {
                    oe = lista.OrderByDescending(m => m.GetType()
                                  .GetProperties()
                                  .First(n => n.Name == coluna)
                                  .GetValue(m, null));
                }

                if (this.orderBy.Count() > 1)
                    oe = AplicarOrdenacao(oe, pos);


                lista = oe.ToList();

                ri.data = lista;
                ri.tipo = lista.GetType();

                return ri;
            }

            ri.data = lista;
            ri.tipo = lista.GetType();

            return ri;
        }

        /// <summary>
        /// Aplica o filtro de paginação e ordenação a uma lista de elementos
        /// </summary>
        /// <typeparam name="T">Defina o tipo do elemento que compoe a lista</typeparam>
        /// <param name="lista">Insira a lista dos objetos</param>
        /// <returns></returns>
        public RetornoInterno AplicarFiltro<T>(List<T> lista)
        {
            RetornoInterno ri = new RetornoInterno();
            List<T> newLista = new List<T>();

            Type t = typeof(T);

            //aplico caso de where
            ri = AplicarWhere<T>(lista);

            lista = ri.data;

            //aplico ordenação dos campos
            ri = AplicarOrdenacao<T>(lista, 0, ri);

            Type tipoRetorno = (Type)ri.tipo;
            lista = (List<T>)ri.data;

            //aplico paginação
            if (this.page > 0 && this.pageSize > 0)
            {
                int tam = lista.Count();
                int partida = (this.page * this.pageSize) - this.pageSize;
                if (partida < 0)
                    partida = 0;
                int chegada = partida + this.pageSize;
                if (chegada > lista.Count())
                    chegada = lista.Count();

                for (int i = partida; i < chegada; i++)
                {
                    newLista.Add(lista[i]);
                }

                ri.data = newLista;
                ri.tipo = newLista.GetType();

                return ri;
            }

            ri.data = lista;
            ri.tipo = lista.GetType();

            return ri;
        }



        public class OrderBy
        {
            public string coluna = "";
            public string sentido = "";

            /// <summary>
            /// 
            /// </summary>
            /// <param name="array">Array com duas posições sendo a primeira o campo e a segunda o sentido.</param>
            public OrderBy(string[] array)
            {
                this.coluna = array[0].Trim();
                this.sentido = array[1].Trim();
            }
        }

        public class Wherer
        {
            public string variavel = "";
            public string expressao = "";
            public dynamic valor = "";

            /// <summary>
            /// 
            /// </summary>
            /// <param name="array">Array com duas posições sendo a primeira o campo e a segunda o sentido.</param>
            public Wherer(string[] array)
            {
                this.variavel = array[0].Trim();
                this.expressao = array[1].Trim();
                if (array.Count() == 3)
                {
                    this.valor = array[2].Trim();
                }
                else
                {
                    string temp = "";
                    for (int i = 2; i < array.Count(); i++)
                    {
                        temp += array[i] + " ";                        
                    }
                    this.valor = temp.ToLower().Trim();
                }
            }
        }
    }
}