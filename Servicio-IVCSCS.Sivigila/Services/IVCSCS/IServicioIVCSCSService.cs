using Servicio_IVCSCS.Sivigila.Models.Request;
using Servicio_IVCSCS.Sivigila.Models.Response;

namespace Servicio_IVCSCS.Sivigila.Services.IVCSCS
{
    public interface IServicioIVCSCSService
    {
        public ConsultarSolicitudResponseDTO consultarSolicitud(ConsultarSolicitudRequestDTO consultarSolicitudRequestDTO);
    }
}
