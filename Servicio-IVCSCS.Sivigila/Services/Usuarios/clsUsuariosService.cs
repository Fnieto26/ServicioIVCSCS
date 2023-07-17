using Microsoft.VisualBasic;
using Servicio_IVCSCS.Sivigila.Services.Encriptar;
using System.Data;
using System.Data.SqlClient;

namespace Servicio_IVCSCS.Sivigila.Services.Usuarios;
public class clsUsuariosService
{
    private readonly IConfiguration _configuration;
    private readonly EncriptarService encriptarService;
    private readonly clsListadoRolesService clsListadoRoles;

    public clsUsuariosService(IConfiguration _configuration, EncriptarService encriptarService, clsListadoRolesService clsListadoRoles)
    {
        _configuration = _configuration;
        this.encriptarService = encriptarService;
        this.clsListadoRoles = clsListadoRoles;
    }


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

    public bool Activo { get; set; }

    private Boolean IsActivo;
    private string ConexionString;
    private string Fax;
    private string Telefono;
    private string RespuestaSecreta;
    private int IdEntidad;
    private int IdSubred;
    private Boolean VerInactivos;
    private string IpUsuario { get; set; }
    private int CambioContrasena;


    public clsUsuariosService(int IdUsuario)
    {
        this.IdUsuario = IdUsuario;
        this.ConexionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }

    public clsUsuariosService(string Login)
    {
        this.Login = Login;
        this.ConexionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

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
    public bool Validate(string Cadena, bool IsContrasena)
    {
        Cadena = encriptarService.Encriptar(Cadena);

        if (IsContrasena = true)
        {
            if (Cadena == Password)
            {
                RegistrarIntentoExitoso();
            }
            else
            {
                RegistrarIntentoFallido();
            }
        }
        else
        {
            if (Cadena.ToUpper() == this.RespuestaSecreta.ToUpper())
            {
                RegistrarIntentoExitoso();
                return true;
            }
            else
            {
                RegistrarIntentoFallido();
                return false;
            }
        }

        return true;
    }

    private void RegistrarIntentoExitoso()
    {
        string query = "SpUpdateLoginExitosoByIdTblUsuario";
        using (SqlConnection objConexion = new SqlConnection(this.ConexionString))
        {
            using (SqlCommand objComando = new SqlCommand(query, objConexion))
            {
                objComando.CommandType = CommandType.StoredProcedure;
                objComando.Parameters.Add(new SqlParameter("@idTblUsuarios", SqlDbType.Int)).Value = this.IdUsuario;

                objComando.Connection.Open();
                objComando.ExecuteNonQuery();
                objComando.Connection.Close();
            }
        }

        this.IntentosActuales = 0;
    }

    private void RegistrarIntentoFallido()
    {
        string query = "SpUpdateIntentosLoginByIdTblUsuario";
        using (SqlConnection objConexion = new SqlConnection(this.ConexionString))
        {
            using (SqlCommand objComando = new SqlCommand(query, objConexion))
            {
                objComando.CommandType = CommandType.StoredProcedure;
                objComando.Parameters.Add(new SqlParameter("@idTblUsuarios", SqlDbType.Int)).Value = this.IdUsuario;

                objConexion.Open();
                objComando.ExecuteNonQuery();
                objConexion.Close();
            }
        }

        this.IntentosActuales += 1;

        // Falta agregar registro en bitácora de intento fallido
        // Insertar registro en Bitacora
        InsertBitacoraSinLogin("SpUpdateIntentosLoginByIdTblUsuario", this.IdUsuario, 732, "Intento=" + IntentosActuales.ToString(), this.IdUsuario);

    }

    public string InsertBitacoraSinLogin(string Procedimiento, object llave, int IdTipoAccion, string ConCatenar, int Usuario)
    {
        SqlConnection conn = new SqlConnection();
        try
        {
            conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["VIGIAConnectionString"].ConnectionString;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spInsertTblBitacoraOK";

            if (llave == DBNull.Value)
            {
                llave = 0;
            }

            cmd.Parameters.Add(new SqlParameter("@Objeto", SqlDbType.Char, 100)).Value = Procedimiento;
            cmd.Parameters.Add(new SqlParameter("@IdRegistroModificado", SqlDbType.Int)).Value = llave;
            cmd.Parameters.Add(new SqlParameter("@IPCliente", SqlDbType.Char, 30)).Value = this.IpUsuario;
            cmd.Parameters.Add(new SqlParameter("@IdtblUsuario", SqlDbType.Int)).Value = Usuario;
            cmd.Parameters.Add(new SqlParameter("@Id_TipoAccion", SqlDbType.Int)).Value = IdTipoAccion;
            cmd.Parameters.Add(new SqlParameter("@IdTblSubsistema", SqlDbType.Int)).Value = 1;
            cmd.Parameters.Add(new SqlParameter("@Parametros", SqlDbType.VarChar, 1000)).Value = ConCatenar;

            conn.Open();

            int Ejecucion;
            Ejecucion = Convert.ToInt32(cmd.ExecuteScalar());

            return "0";
        }
        catch (Exception ex)
        {
            return "-1";
        }
        finally
        {
            conn.Close();
        }
    }

    public Collection GetRoles()
    {
        Collection ObjCollection = new Collection();

        string query = "SpGetRolesByIdTblUsuarios";
        using (SqlConnection objConexion = new SqlConnection(this.ConexionString))
        {
            using (SqlCommand objComando = new SqlCommand(query, objConexion))
            {
                objComando.CommandType = CommandType.StoredProcedure;
                objComando.Parameters.Add(new SqlParameter("@idTblUsuarios", SqlDbType.Int)).Value = this.IdUsuario;

                objConexion.Open();

                SqlDataReader ObjLector = objComando.ExecuteReader();

                while (ObjLector.Read())
                {
                    clsListadoRolesService Rol = new clsListadoRolesService(Convert.ToInt32(ObjLector["IdTblRoles"]), ObjLector["NombreRol"].ToString());
                    ObjCollection.Add(Rol);
                }

                ObjLector.Close();
            }
        }

        return ObjCollection;
    }

    public Collection GetLocalidades()
    {
        Collection ObjCollection = new Collection();
        string query = "SpGetLocalidadesByIdTblEntidad";
        using (SqlConnection objConexion = new SqlConnection(this.ConexionString))
        {
            using (SqlCommand objComando = new SqlCommand(query, objConexion))
            {
                objComando.CommandType = CommandType.StoredProcedure;
                objComando.Parameters.Add(new SqlParameter("@IdTblEntidad", SqlDbType.Int)).Value = this.IdEntidad;
                objConexion.Open();

                //SqlDataReader ObjLector = objComando.ExecuteReader();
                //while (ObjLector.Read())
                //{
                //    Quasar.Vigia.BL.datosSession.localidades nuevaLocalidad = new Quasar.Vigia.BL.datosSession.localidades();
                //    nuevaLocalidad.idLocalidad = Convert.ToInt32(ObjLector["IdTblLocalidades"]);
                //    nuevaLocalidad.nombreLocalidad = ObjLector["NombreLocalidad"].ToString();
                //    nuevaLocalidad.CodLocalidad = ObjLector["CodLocalidad"].ToString();
                //    ObjCollection.Add(nuevaLocalidad);
                //}
                //ObjLector.Close();
            }
        }

        return ObjCollection;
    }

    public string ChangePassword()
    {
        string strClave;
        long Semilla = 232345;
        Random random = new Random();
        strClave = Convert.ToString((int)((Semilla * 3 * random.NextDouble()) + Semilla));

        string query = "SpUpdateContrasenaByIdTblUsuario";
        using (SqlConnection objConexion = new SqlConnection(this.ConexionString))
        {
            using (SqlCommand objComando = new SqlCommand(query, objConexion))
            {
                objComando.CommandType = CommandType.StoredProcedure;
                objComando.Parameters.Add(new SqlParameter("@contrasena", SqlDbType.VarChar)).Value = encriptarService.Encriptar(strClave);
                objComando.Parameters.Add(new SqlParameter("@CambioContrasena", SqlDbType.Bit)).Value = true;
                objComando.Parameters.Add(new SqlParameter("@idTblUsuarios", SqlDbType.Int)).Value = this.IdUsuario;

                objConexion.Open();
                objComando.ExecuteNonQuery();
                objConexion.Close();
            }
        }
        return strClave;
    }
    public string GenerarPassword()
    {
        string strClave;
        long Semilla = 232345;
        Random random = new Random();
        strClave = Convert.ToString((int)((Semilla * 3 * random.NextDouble()) + Semilla));
        return strClave;
    }

    public void ChangePasswordCorreo(string sClave)
    {
        string query = "SpUpdateContrasenaByIdTblUsuario";
        using (SqlConnection objConexion = new SqlConnection(this.ConexionString))
        {
            using (SqlCommand objComando = new SqlCommand(query, objConexion))
            {
                objComando.CommandType = CommandType.StoredProcedure;
                objComando.Parameters.Add(new SqlParameter("@contrasena", SqlDbType.VarChar)).Value = encriptarService.Encriptar(sClave);
                objComando.Parameters.Add(new SqlParameter("@CambioContrasena", SqlDbType.Bit)).Value = true;
                objComando.Parameters.Add(new SqlParameter("@idTblUsuarios", SqlDbType.Int)).Value = this.IdUsuario;

                objConexion.Open();
                objComando.ExecuteNonQuery();
                objConexion.Close();
            }
        }
    }

    public bool ChangePassword(string NewPassword, string OldPassword/*, HttpSessionState ObjSession*/)
    {
        bool Cambio;

        if (Validate(OldPassword, true))
        {
            string query = "SpUpdateContrasenaByIdTblUsuario";
            using (SqlConnection objConexion = new SqlConnection(this.ConexionString))
            {
                using (SqlCommand objComando = new SqlCommand(query, objConexion))
                {
                    objComando.CommandType = CommandType.StoredProcedure;
                    objComando.Parameters.Add(new SqlParameter("@contrasena", SqlDbType.VarChar, 100)).Value = encriptarService.Encriptar(NewPassword);
                    objComando.Parameters.Add(new SqlParameter("@CambioContrasena", SqlDbType.Bit)).Value = false;
                    objComando.Parameters.Add(new SqlParameter("@idTblUsuarios", SqlDbType.Int)).Value = this.IdUsuario;

                    objConexion.Open();
                    objComando.ExecuteNonQuery();
                    objConexion.Close();
                }
            }

            // Insertar registro en Bitacora
            //clsBitacoraService.RegistrarEnBitacora("SpUpdateContrasenaByIdTblUsuario", this._IdUsuario, ObjSession, 731, "");

            Cambio = true;
        }
        else
        {
            Cambio = false;
        }

        return Cambio;
    }

    public string GetLoginUsuario()
    {
        string query = "spGetTblUsuariosLogin";
        using (SqlConnection objConexion = new SqlConnection(ConexionString))
        {
            using (SqlCommand objComando = new SqlCommand(query, objConexion))
            {
                objComando.Parameters.Add(new SqlParameter("@IdUsuario", SqlDbType.Int)).Value = IdUsuario;
                objComando.CommandType = CommandType.StoredProcedure;

                try
                {
                    string strRespuesta;
                    objConexion.Open();
                    strRespuesta = Convert.ToString(objComando.ExecuteScalar());
                    objConexion.Close();

                    if (strRespuesta != null)
                    {
                        return strRespuesta.Trim();
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }

    public bool ModificarUsuarisoByTelefonoFax(bool IsUpdatePregunta)
    {
        string query = "spUpdateTblUsuariosFaxTelefono";
        using (SqlConnection objConexion = new SqlConnection(ConexionString))
        {
            using (SqlCommand objComando = new SqlCommand(query, objConexion))
            {
                objComando.Parameters.Add(new SqlParameter("@IdUsuario", SqlDbType.Int)).Value = IdUsuario;
                objComando.Parameters.Add(new SqlParameter("@Telefono", SqlDbType.Char, 15)).Value = Telefono;
                objComando.Parameters.Add(new SqlParameter("@Fax", SqlDbType.Char, 15)).Value = Fax;

                if (IsUpdatePregunta)
                {
                    objComando.Parameters.Add(new SqlParameter("@PreguntaSecreta", SqlDbType.VarChar, 100)).Value = Pregunta;
                    objComando.Parameters.Add(new SqlParameter("@Respuesta", SqlDbType.VarChar, 100)).Value = RespuestaSecreta;
                    objComando.Parameters.Add(new SqlParameter("@All", SqlDbType.Int)).Value = 1;
                }
                else
                {
                    objComando.Parameters.Add(new SqlParameter("@PreguntaSecreta", SqlDbType.VarChar, 100)).Value = DBNull.Value;
                    objComando.Parameters.Add(new SqlParameter("@Respuesta", SqlDbType.VarChar, 100)).Value = DBNull.Value;
                    objComando.Parameters.Add(new SqlParameter("@All", SqlDbType.Int)).Value = 0;
                }

                objComando.CommandType = CommandType.StoredProcedure;

                try
                {
                    objConexion.Open();
                    if (objComando.ExecuteNonQuery() > 0)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    objConexion.Close();
                }
            }
        }
    }


}
