using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace DoctorAppointmentSystem.HelperClasses
{
    public class EmailSender
    {
        private string smtpServer;
        private int smtpPort;
        private string smtpUsername;
        private string smtpPassword;

        public EmailSender()
        {
            smtpServer = "smtp.gmail.com";
            smtpPort = 587;
            smtpUsername = "appointment.cusc@gmail.com";
            smtpPassword = "mbkaaidugrcppbog";
        }

        public static string GenerateActivationToken(string username)
        {
            string token = PasswordHelper.HashPassword(username.Trim()).Trim();
            return token;
        }

        /// <summary>
        /// Return a link that includes a token that lasts for 5 minutes
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string GenerateActivationLink(string token)
        {
            DateTime currentTime = DateTime.Now;

            var httpContext = HttpContext.Current;
            var request = httpContext.Request;
            var baseUrl = request.Url.GetLeftPart(UriPartial.Authority);

            DateTime expirationTime = currentTime.AddDays(1);
            string expirationTimestamp = ((DateTimeOffset)expirationTime).ToUnixTimeSeconds().ToString();

            string activationLink = $"{baseUrl}/Account/Activate?token={token}&expires={expirationTimestamp}";

            return activationLink;
        }

        public async Task SendActivationEmailAsync(string recipientEmail, string activationLink)
        {
            try
            {
                await Task.Run(() =>
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);

                    mail.From = new MailAddress(smtpUsername);
                    mail.To.Add(recipientEmail);
                    mail.Subject = "Activate your account";
                    mail.Body = $"Please click the following link to activate your account: {activationLink}";
                    mail.IsBodyHtml = true;

                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtpClient.EnableSsl = true;

                    smtpClient.Send(mail);
                });
            }
            catch (Exception)
            {
                // Handle email sending failure
                string sEventCatg = "PATIENT PORTAL";
                string sEventMsg = "Exception: Unable to send activation email for the account.";
                string sEventSrc = "ActivateAcc";
                string sEventType = "S";
                string sInsBy = recipientEmail;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
            }
        }
    }
}