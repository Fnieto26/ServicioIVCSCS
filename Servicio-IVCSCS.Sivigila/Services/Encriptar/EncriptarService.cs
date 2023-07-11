using System.Security.Cryptography;
using System.Text;

namespace Servicio_IVCSCS.Sivigila.Services.Encriptar
{
    public class EncriptarService
    {
        public string Encriptar(string ValorAEncriptar)
        {
            byte[] ValorToHash = ConvertirCadenaEnMatrizDeTipoByte(ValorAEncriptar);
            byte[] ValorHash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(ValorToHash);
            return Convert.ToBase64String(ValorHash);
        }

        public static byte[] ConvertirCadenaEnMatrizDeTipoByte(string Cadena)
        {
            return Encoding.Unicode.GetBytes(Cadena);
        }
    }
}
