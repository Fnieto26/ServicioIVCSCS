namespace Servicio_IVCSCS.Sivigila.Models.Response
{
    public class ConsultarSolicitudResponseDTO
    {
        public string CodError { get; set; }
        public string TextoError { get; set; }
        public string Id_EstadoSolicitud { get; set; }
        public string Des_EstadoSolicitud { get; set; }
        public string Respuesta { get; set; }
        public string FechaRespuesta { get; set; }
        public string ConceptoSanitario { get; set; }
        public string FechaProgramada { get; set; }
    }
}
