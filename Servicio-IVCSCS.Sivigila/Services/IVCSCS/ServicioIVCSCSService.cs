using Nancy.Json;
using Servicio_IVCSCS.Sivigila.Models.Request;
using Servicio_IVCSCS.Sivigila.Models.Response;
using Servicio_IVCSCS.Sivigila.Services.Encriptar;
using Servicio_IVCSCS.Sivigila.Services.Usuarios;

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

            //objUsuario.
            throw new NotImplementedException();
        }



    }

}
