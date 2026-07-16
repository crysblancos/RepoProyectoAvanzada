using Proyecto_Grupo02.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Proyecto_Grupo02.Services
{
    public class UtilitarioService
    {
        // Registra errores en la tabla tbError usando LINQ (sin stored procedure)
        public void RegistrarErrorBitacora(string mensaje, string lugar)
        {
            try
            {
                using (var context = new KA_FASHION_BDEntities())
                {
                    var usuario = 0;
                    if (HttpContext.Current.Session["ConsecutivoUsuario"] != null)
                        usuario = (int)HttpContext.Current.Session["ConsecutivoUsuario"];

                    context.tbError.Add(new tbError
                    {
                        Mensaje = mensaje,
                        FechaHora = DateTime.Now,
                        Lugar = lugar,
                        ConsecutivoUsuario = usuario
                    });

                    context.SaveChanges();
                }
            }
            catch
            {
                // Si falla el registro del error, no queremos que la app se caiga por esto
            }
        }

        // Genera una contraseña temporal aleatoria
        public string GenerarContrasenna()
        {
            var random = new Random();
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const string especiales = "!@#$%&*";

            char[] password = Enumerable.Repeat(caracteres, 8)
                .Select(s => s[random.Next(s.Length)])
                .ToArray();

            password[random.Next(password.Length)] = especiales[random.Next(especiales.Length)];

            return new string(password);
        }

        // Envía correo por Gmail
        public void EnviarCorreo(string destinatario, string asunto, string cuerpoHtml)
        {
            var correoSalida = ConfigurationManager.AppSettings["CorreoSalida"];
            var contrasennaCorreoSalida = ConfigurationManager.AppSettings["ContrasennaCorreoSalida"];

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(correoSalida);
                mail.To.Add(destinatario);
                mail.Subject = asunto;
                mail.Body = cuerpoHtml;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(correoSalida, contrasennaCorreoSalida);
                    smtp.EnableSsl = true;

                    if (!string.IsNullOrEmpty(contrasennaCorreoSalida))
                    {
                        smtp.Send(mail);
                    }
                }
            }
        }
    }
}