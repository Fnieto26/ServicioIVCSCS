namespace Servicio_IVCSCS.Sivigila.Services.Usuarios
{
    public class clsListadoRolesService
    {
        private int _IdRol;
        private string _Nombre;

        public clsListadoRolesService(int Id, string Nombre)
        {
            this._IdRol = Id;
            this._Nombre = Nombre;
        }

        public string Nombre
        {
            get { return this._Nombre; }
        }

        public int IdRol
        {
            get { return this._IdRol; }
        }
    }
}
