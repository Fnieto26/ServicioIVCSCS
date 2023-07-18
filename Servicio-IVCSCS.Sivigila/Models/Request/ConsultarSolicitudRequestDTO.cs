namespace Servicio_IVCSCS.Sivigila.Models.Request
{
    public class ConsultarSolicitudRequestDTO
    {
        public string pLogin { get; set; }
        public string pContrasena { get; set; }
        public string NroSoicitud { get; set; }


        public ConsultarSolicitudRequestDTO(string login, string pContrasena, string NroSoicitud)
        {
            pLogin = login;
            pContrasena = pContrasena;
            NroSoicitud = NroSoicitud;
        }
    }
}
