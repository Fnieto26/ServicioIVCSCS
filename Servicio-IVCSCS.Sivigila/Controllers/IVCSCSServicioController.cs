using Microsoft.AspNetCore.Mvc;
using Servicio_IVCSCS.Sivigila.Models.Request;
using Servicio_IVCSCS.Sivigila.Models.Response;
using Servicio_IVCSCS.Sivigila.Services.IVCSCS;

namespace Servicio_IVCSCS.Sivigila.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IVCSCSServicioController : ControllerBase
    {
        private readonly IServicioIVCSCSService servicioIVCSCSService;

        public IVCSCSServicioController(IServicioIVCSCSService servicioIVCSCSService)
        {
            this.servicioIVCSCSService = servicioIVCSCSService;            
        }


        [HttpGet("Servicio-IVCSCS-Sivigila-Consultar-Solicitud")]

        public ConsultarSolicitudResponseDTO consultarSolicitud(ConsultarSolicitudRequestDTO consultarSolicitudRequestDTO)
        {
            return null;
        }
    }
}