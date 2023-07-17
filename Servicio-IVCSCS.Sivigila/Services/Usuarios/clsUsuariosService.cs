namespace Servicio_IVCSCS.Sivigila.Services.Usuarios;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class clsUsuariosService
{
    // "Propiedades"
    public int IdUsuario { get; set; }
    private string Login;
    private string Password { get; set; }
    private int IntentosMaximos;
    private int IntentosActuales;
    private int NumeroMaximoClaves;
    private int ConteoCambioClave;
    private string Pregunta;
    private string Email;

    public bool Activo { get;  set; }

    private Boolean IsActivo;
    private string ConexionString;
    private string Fax;
    private string Telefono;
    private string RespuestaSecreta;
    private int IdEntidad;
    private int IdSubred;
    private Boolean VerInactivos;
    private string IpUsuario;
    private int CambioContrasena;


    public clsUsuariosService(int IdUsuario)
    {
        this.IdUsuario = IdUsuario;
        this.ConexionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }

    public clsUsuariosService(string Login)
    {
        this.Login = Login;
        this.ConexionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        ObtenerDatos();
    }

    private void ObtenerDatos()
    {
        string query = "spGetDatosUsuarioByLogin";
        SqlConnection objConexion = new SqlConnection(this.ConexionString);
        SqlCommand objComando = new SqlCommand(query, objConexion);
        objComando.CommandType = CommandType.StoredProcedure;

        objComando.Parameters.Add(new SqlParameter("@Login", SqlDbType.VarChar)).Value = this.Login;
        objComando.Parameters.Add(new SqlParameter("@Activo", SqlDbType.Bit)).Value = true;

        DataSet objDataSet = new DataSet();
        SqlDataAdapter ObjAdapter = new SqlDataAdapter(objComando);

        try
        {
            ObjAdapter.Fill(objDataSet);

            if (objDataSet.Tables[0].Rows.Count > 0)
            {
                this.IdUsuario = (int)objDataSet.Tables[0].Rows[0]["IdTblUsuarios"];
                this.Password = (string)objDataSet.Tables[0].Rows[0]["Contrasena"];

                if (!Convert.IsDBNull(objDataSet.Tables[0].Rows[0]["NumeroMaximoIntentosLogin"]))
                    this.IntentosMaximos = (int)objDataSet.Tables[0].Rows[0]["NumeroMaximoIntentosLogin"];

                if (!Convert.IsDBNull(objDataSet.Tables[0].Rows[0]["IntentosFallidosLogin"]))
                    this.IntentosActuales = (int)objDataSet.Tables[0].Rows[0]["IntentosFallidosLogin"];
                else
                    this.IntentosActuales = 0;

                if (!Convert.IsDBNull(objDataSet.Tables[0].Rows[0]["NumeroMaximoClaves"]))
                    this.NumeroMaximoClaves = (int)objDataSet.Tables[0].Rows[0]["NumeroMaximoClaves"];
                else
                    this.NumeroMaximoClaves = 0;

                if (!Convert.IsDBNull(objDataSet.Tables[0].Rows[0]["ConteoCambioClave"]))
                    this.ConteoCambioClave = (int)objDataSet.Tables[0].Rows[0]["ConteoCambioClave"];
                else
                    this.ConteoCambioClave = 0;

                if (!Convert.IsDBNull(objDataSet.Tables[0].Rows[0]["CambioContrasena"]))
                    this.CambioContrasena = (int)objDataSet.Tables[0].Rows[0]["CambioContrasena"];
                else
                    this.CambioContrasena = 0;

                if (!Convert.IsDBNull(objDataSet.Tables[0].Rows[0]["Pregunta"]))
                    this.Pregunta = (string)objDataSet.Tables[0].Rows[0]["Pregunta"];

                if (!Convert.IsDBNull(objDataSet.Tables[0].Rows[0]["Respuesta"]))
                    this.RespuestaSecreta = (string)objDataSet.Tables[0].Rows[0]["Respuesta"];

                this.Email = (string)objDataSet.Tables[0].Rows[0]["Email"];
                this.Activo = true;

                if (!Convert.IsDBNull(objDataSet.Tables[0].Rows[0]["IdTblEntidades"]))
                    this.IdEntidad = (int)objDataSet.Tables[0].Rows[0]["IdTblEntidades"];

                if (!Convert.IsDBNull(objDataSet.Tables[0].Rows[0]["id_subred"]))    
                    this.IdSubred = (int)objDataSet.Tables[0].Rows[0]["id_subred"];
            }
            else
            {
                this.Activo = false;
            }
        }
        catch (Exception ex)
        {
            this.Activo = false;
        }
        finally
        {
            objConexion.Close();
        }
    }

}
