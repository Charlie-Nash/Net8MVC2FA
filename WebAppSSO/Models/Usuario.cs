namespace WebAppSSO.Models
{
    public class Usuario
    {
        public required string Nick { get; set; }
        public required string PwdHash { get; set; }
        public bool Requiere2FA { get; set; }
        public bool RequiereQR { get; set; }
        public string? SecretKey { get; set; }
    }
}
