using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OtpNet;
using WebAppSSO.Models;
using WebAppSSO.Helpers;

namespace WebAppSSO.Controllers
{
    public class LoginController : Controller
    {
        private readonly UsuarioList _usuarioList;

        public LoginController(UsuarioList usuarioList)
        {
            _usuarioList = usuarioList;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            var usuario = _usuarioList.Usuarios.FirstOrDefault(u => u.Nick == username);

            if (usuario == null || password == null || !BCrypt.Net.BCrypt.Verify(password, usuario.PwdHash))
            {
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                return View();
            }

            if (usuario.Requiere2FA)
            {
                TempData["username"] = username;

                return RedirectToAction("TwoFactor");
            }

            return RedirectToAction("Autenticado");
        }

        [HttpGet]
        public IActionResult TwoFactor()
        {
            var nick = TempData["username"] as string;

            if (nick == null) return RedirectToAction("Index");

            var usuario = _usuarioList.Usuarios.FirstOrDefault(u => u.Nick == nick);

            if (usuario == null) return RedirectToAction("Index");

            ViewBag.Username = nick;
            ViewBag.QRCodeImage = "";

            if (usuario.RequiereQR)
            {
                ViewBag.QRCodeImage = QR.GenerateCodeUri(nick, usuario.SecretKey);
            }            

            return View();
        }

        [HttpPost]
        public IActionResult TwoFactor(string username, string code)
        {
            var usuario = _usuarioList.Usuarios.FirstOrDefault(u => u.Nick == username);

            if (usuario == null || string.IsNullOrWhiteSpace(code))
            {
                return RedirectToAction("Index");
            }                

            var totp = new Totp(Base32Encoding.ToBytes(usuario.SecretKey));

            if (totp.VerifyTotp(code, out _, new VerificationWindow(2, 2)))
            {
                TempData["nick"] = usuario.Nick;
                TempData["pwd"] = usuario.PwdHash;

                usuario.RequiereQR = false;

                return RedirectToAction("Autenticado");
            }

            ViewBag.QRCodeImage = "";
            ViewBag.Username = username;

            if (usuario.RequiereQR)
            {
                ViewBag.QRCodeImage = QR.GenerateCodeUri(username, usuario.SecretKey);
            }
                
            ViewBag.Error = "Código incorrecto.";

            return View();
        }

        public IActionResult Autenticado()
        {
            var nick = TempData["nick"] as string;
            var pwd = TempData["pwd"] as string;

            ViewBag.Username = nick;
            ViewBag.PwdHash = pwd;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
