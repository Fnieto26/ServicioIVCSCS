using System.Data;

namespace Servicio_IVCSCS.Sivigila.Services.Usuarios
{
    public class CmdParametro
    {
        public string Nombre { get; set; }
        public object Valor { get; set; }
        public SqlDbType TipoDato { get; set; }
        public bool Requerido { get; set; }

        public CmdParametro(string nombre, object valor, SqlDbType tipoDato, bool requerido)
        {
            Nombre = nombre;
            Valor = valor;
            TipoDato = tipoDato;
            Requerido = requerido;
        }
    }
}
