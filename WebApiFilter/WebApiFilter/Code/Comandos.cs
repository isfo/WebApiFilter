using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WebApiFilter.Models;

namespace WebApiFilter.Code
{
    public class Comandos
    {
        public static List<Usuario> GerarUsuarios()
        {
            List<Usuario> lista = new List<Usuario>();

            Random rnd = new Random();

            lista.Add(new Usuario(1, "Breno Vinicius Fernandes", DateTime.Now.AddMonths(-rnd.Next(12)).AddDays(-rnd.Next(360)).AddYears(-rnd.Next(13, 72)), rnd.Next(999999999).ToString() + rnd.Next(10, 99).ToString(), rnd.Next(150, 210), "M"));
            lista.Add(new Usuario(2, "Henrique Hugo Souza", DateTime.Now.AddMonths(-rnd.Next(12)).AddDays(-rnd.Next(360)).AddYears(-rnd.Next(13, 72)), rnd.Next(999999999).ToString() + rnd.Next(10, 99).ToString(), rnd.Next(150, 210), "M"));
            lista.Add(new Usuario(3, "Giovanna Lavínia dos Santos", DateTime.Now.AddMonths(-rnd.Next(12)).AddDays(-rnd.Next(360)).AddYears(-rnd.Next(13, 72)), rnd.Next(999999999).ToString() + rnd.Next(10, 99).ToString(), rnd.Next(150, 210), "F"));
            lista.Add(new Usuario(4, "Julia Larissa Pereira", DateTime.Now.AddMonths(-rnd.Next(12)).AddDays(-rnd.Next(360)).AddYears(-rnd.Next(13, 72)), rnd.Next(999999999).ToString() + rnd.Next(10, 99).ToString(), rnd.Next(150, 210), "F"));
            lista.Add(new Usuario(5, "Rodrigo Carlos Eduardo Oliveira", DateTime.Now.AddMonths(-rnd.Next(12)).AddDays(-rnd.Next(360)).AddYears(-rnd.Next(13, 72)), rnd.Next(999999999).ToString() + rnd.Next(10, 99).ToString(), rnd.Next(150, 210), "M"));
            lista.Add(new Usuario(6, "Otávio Nicolas Souza", DateTime.Now.AddMonths(-rnd.Next(12)).AddDays(-rnd.Next(360)).AddYears(-rnd.Next(13, 72)), rnd.Next(999999999).ToString() + rnd.Next(10, 99).ToString(), rnd.Next(150, 210), "M"));
            lista.Add(new Usuario(7, "Beatriz Bruna Helena Pinto", DateTime.Now.AddMonths(-rnd.Next(12)).AddDays(-rnd.Next(360)).AddYears(-rnd.Next(13, 72)), rnd.Next(999999999).ToString() + rnd.Next(10, 99).ToString(), rnd.Next(150, 210), "F"));
            lista.Add(new Usuario(8, "Eduarda Gabrielly Fernandes", DateTime.Now.AddMonths(-rnd.Next(12)).AddDays(-rnd.Next(360)).AddYears(-rnd.Next(13, 72)), rnd.Next(999999999).ToString() + rnd.Next(10, 99).ToString(), rnd.Next(150, 210), "F"));
            lista.Add(new Usuario(9, "Lucca Filipe Dias", DateTime.Now.AddMonths(-rnd.Next(12)).AddDays(-rnd.Next(360)).AddYears(-rnd.Next(13, 72)), rnd.Next(999999999).ToString() + rnd.Next(10, 99).ToString(), rnd.Next(150, 210), "M"));
            lista.Add(new Usuario(10, "Maria Giovanna Oliveira", DateTime.Now.AddMonths(-rnd.Next(12)).AddDays(-rnd.Next(360)).AddYears(-rnd.Next(13, 72)), rnd.Next(999999999).ToString() + rnd.Next(10, 99).ToString(), rnd.Next(150, 210), "F"));


            return lista;
        }

        public static class Base64
        {
            public static string ToBase64(string text)
            {
                if (text == null)
                {
                    return null;
                }

                byte[] textAsBytes = Encoding.UTF8.GetBytes(text);
                return System.Convert.ToBase64String(textAsBytes);
            }

            public static string FromBase64(string encodedText)
            {
                if (encodedText == null)
                {
                    return null;
                }

                byte[] textAsBytes = System.Convert.FromBase64String(encodedText);
                return Encoding.UTF8.GetString(textAsBytes);
            }
        }

        /// <summary>
        /// Reune métodos de conversão com tratamento para exception
        /// </summary>
        public static class Converter
        {
            public static class CastHelper
            {
                /// <summary>
                /// Realiza o cast de um objeto para determinado type
                /// </summary>
                /// <param name="src"></param>
                /// <param name="t"></param>
                /// <returns></returns>
                public static dynamic Cast(dynamic src, Type t)
                {
                    var castMethod = typeof(CastHelper).GetMethod("CastGeneric").MakeGenericMethod(t);
                    return castMethod.Invoke(null, new[] { src });
                }
                public static T CastGeneric<T>(object src)
                {

                    Type t = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

                    object safeValue = (src == null) ? null : Convert.ChangeType(src, t);
                    if (safeValue == null)
                        return default(T);
                    else
                        return (T)Convert.ChangeType(src, t);
                }
            }

            public static int toInt(object value)
            {
                int valor;
                try
                {
                    valor = Convert.ToInt32(value);
                }
                catch
                {
                    throw new Exception("Não foi possível converter '" + value.ToString() + "' para INT.");
                }

                return valor;
            }

            public static long toLong(object value)
            {
                long valor;
                try
                {
                    valor = Convert.ToInt64(value);
                }
                catch
                {
                    throw new Exception("Não foi possível converter '" + value.ToString() + "' para LONG.");
                }

                return valor;
            }
        }

    }
}