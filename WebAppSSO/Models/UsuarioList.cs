using OtpNet;

namespace WebAppSSO.Models
{
    public class UsuarioList
    {
        public List<Usuario> Usuarios { get; set; }

        public UsuarioList() 
        {
            Usuarios = new List<Usuario>
            {
                new Usuario
                {
                    Nick = "admin",
                    PwdHash = BCrypt.Net.BCrypt.HashPassword("1234"),
                    Requiere2FA = true,
                    RequiereQR = true,
                    SecretKey = Base32Encoding.ToString(KeyGeneration.GenerateRandomKey(20))
                },
                new Usuario
                {
                    Nick = "cpenac",
                    PwdHash = BCrypt.Net.BCrypt.HashPassword("1234"),
                    Requiere2FA = true,
                    RequiereQR = true,
                    SecretKey = Base32Encoding.ToString(KeyGeneration.GenerateRandomKey(20))
                },
                new Usuario
                {
                    Nick = "aestrada",
                    PwdHash = BCrypt.Net.BCrypt.HashPassword("1234"),
                    Requiere2FA = true,
                    RequiereQR = true,
                    SecretKey = Base32Encoding.ToString(KeyGeneration.GenerateRandomKey(20))
                }
            };
        }
    }
}
