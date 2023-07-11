using Servicio_IVCSCS.Sivigila.Models.Request;
using Servicio_IVCSCS.Sivigila.Models.Response;
using Servicio_IVCSCS.Sivigila.Services.Encriptar;
using System.Security.Cryptography;
using System.Text;

namespace Servicio_IVCSCS.Sivigila.Services.IVCSCS
{
    public class ServicioIVCSCSService : IServicioIVCSCSService
    {
        private readonly IConfiguration _configuration;
        private readonly EncriptarService encriptarService;

        public ServicioIVCSCSService(IConfiguration configuration, EncriptarService encriptarService)
        {
            _configuration = configuration;
            this.encriptarService = encriptarService;
        }
        public ConsultarSolicitudResponseDTO consultarSolicitud(ConsultarSolicitudRequestDTO consultarSolicitudRequestDTO)
        {
            ConsultarSolicitudResponseDTO response = new ConsultarSolicitudResponseDTO();

            response.CodError = "0";
            response.TextoError = "";

            string valorEncriptado = encriptarService.Encriptar(consultarSolicitudRequestDTO.pContrasena);
            string sJSON;

            throw new NotImplementedException();
        }
        


    }

}
