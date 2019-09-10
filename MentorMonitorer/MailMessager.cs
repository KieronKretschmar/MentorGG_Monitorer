using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MentorMonitorer
{
    /// <summary>
    /// Set auth data as environment variable, 
    /// see https://www.twilio.com/docs/usage/secure-credentials and https://www.twilio.com/blog/2017/01/how-to-set-environment-variables.html
    /// </summary>
    class MailMessager
    {
        // Gandi Mail related settings
        private static SmtpClient SmtpServer;
        private static string GandiUserName;
        private static string GandiPassword;


        static MailMessager()
        {
            // Gandi Mail related settings
            GandiUserName = System.Environment.GetEnvironmentVariable("GANDI_MAIL_USERNAME", EnvironmentVariableTarget.User);
            GandiPassword = System.Environment.GetEnvironmentVariable("GANDI_MAIL_PASSWORD", EnvironmentVariableTarget.User);
            SmtpServer = new SmtpClient("mail.gandi.net");
            SmtpServer.Port = 587;
            SmtpServer.Credentials =
            new System.Net.NetworkCredential(GandiUserName, GandiPassword);
            SmtpServer.EnableSsl = true;
        }

        public static void SendMail(string subject, string message, string receiverAdress)
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(GandiUserName);
            mail.To.Add(receiverAdress);
            mail.Subject = subject;
            mail.Body = message;

            SmtpServer.Send(mail);
        }
    }
}
