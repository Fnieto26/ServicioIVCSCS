using Microsoft.VisualBasic;
using Nancy.Json;
using Servicio_IVCSCS.Sivigila.Models.Request;
using Servicio_IVCSCS.Sivigila.Models.Response;
using Servicio_IVCSCS.Sivigila.Services.Encriptar;
using Servicio_IVCSCS.Sivigila.Services.Usuarios;
using System.Data;

namespace Servicio_IVCSCS.Sivigila.Services.IVCSCS
{
    public class ServicioIVCSCSService : IServicioIVCSCSService
    {
        private readonly IConfiguration _configuration;
        private readonly EncriptarService encriptarService;
        private readonly clsUsuariosService clsUsuariosService;

        public ServicioIVCSCSService(IConfiguration configuration, EncriptarService encriptarService, clsUsuariosService clsUsuariosService)
        {
            _configuration = configuration;
            this.encriptarService = encriptarService;
            this.clsUsuariosService = clsUsuariosService;
        }
        public ConsultarSolicitudResponseDTO consultarSolicitud(ConsultarSolicitudRequestDTO consultarSolicitudRequestDTO)
        {
            ConsultarSolicitudResponseDTO response = new ConsultarSolicitudResponseDTO();

            response.CodError = "0";
            response.TextoError = "";

            string valorEncriptado = encriptarService.Encriptar(consultarSolicitudRequestDTO.pContrasena);
            string sJSON;
            JavaScriptSerializer js = new JavaScriptSerializer(); //tener en cuenta el nugget

            clsUsuariosService objUsuario = new clsUsuariosService(consultarSolicitudRequestDTO.pLogin);

            //objUsuario.IpUsuario = HttpContext.Current.Request.UserHostAddress;

            if (objUsuario.Activo == false)
            {
                response.CodError = "-1";
                response.TextoError = "Usuario no encontrado o no activo en el sistema. Contacte al Administrador de la aplicación";

                // Se graba en Bitacora cada intento de ingreso con un usuario que no esté registrado
                // y se graba con el usuario 1
                objUsuario.InsertBitacoraSinLogin("", DBNull.Value, 734, "Login= " + consultarSolicitudRequestDTO.pLogin.Trim(), 1);

                js = new JavaScriptSerializer();
                sJSON = js.Serialize(response);

                return response;
            }

            if (objUsuario.Validate(consultarSolicitudRequestDTO.pContrasena, true) == true)
            {
                // Login Exitoso
                clsUsuariosService.IdUsuario = objUsuario.IdUsuario;
                //Session["UserRoles"] = objUsuario.GetRoles();
                clsUsuariosService.IpUsuario = objUsuario.IpUsuario;
                //Session["IdSubsistema"] = 4;
                clsUsuariosService.IdEntidad = objUsuario.IdEntidad;
                //Session["Localidades"] = objUsuario.GetLocalidades();
                clsUsuariosService.VerInactivos = objUsuario.VerInactivos;
            }
            else
            {
                response.CodError = "";
                response.TextoError = "Usuario o contraseña inválida";

                js = new JavaScriptSerializer();
                sJSON = js.Serialize(response);
                // return sJSON;
                return response;
            }

            if (consultarSolicitudRequestDTO.NroSoicitud.Trim() == "")
            {
                response.CodError = "";
                response.TextoError = "Número de solicitud visita es requerido";
                return response;
            }

            DataSet ds = new DataSet();
            DataRow Dr = null;
            Collection ObjParametros = new Collection();
            CmdParametro ObjItems = new CmdParametro("NroSolicitud", consultarSolicitudRequestDTO.NroSoicitud, SqlDbType.BigInt, false);

            ObjParametros.Add(ObjItems);
            //ds = clsBitacora.GetSelect("spGetTblQuejasByNroSolicitud", ObjParametros, HttpContext.Current.Session, 35, false);

            


            throw new NotImplementedException();
        }



    }

}
