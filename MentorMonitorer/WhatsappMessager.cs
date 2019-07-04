using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace MentorMonitorer
{
    /// <summary>
    /// Set auth data as environment variable, 
    /// see https://www.twilio.com/docs/usage/secure-credentials and https://www.twilio.com/blog/2017/01/how-to-set-environment-variables.html
    /// </summary>
    class Messager
    {

        //// Twilio phone related settings
        //public static string PhoneNumberTo;
        //public static string PhoneNumberFrom;
        //public static string RequestUri;
        //public static string AccountSid;
        //public static string AuthToken;

        // Gandi Mail related settings
        private static SmtpClient SmtpServer;
        private static string GandiUserName;
        private static string GandiPassword;


        static Messager()
        {
            //// Twilio phone related settings
            //PhoneNumberTo = "+491751571271";
            //PhoneNumberFrom = "+14155238886";
            //AccountSid = System.Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID", EnvironmentVariableTarget.User);
            //AuthToken = System.Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN", EnvironmentVariableTarget.User);
            //RequestUri = "https://api.twilio.com/2010-04-01/Accounts/AC2b8d4d2a3e95907f2779a9f723248b28/Messages.json";

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

        //public static void SendWhatsAppMessage(string text)
        //{
        //    // Find your Account Sid and Token at twilio.com/console
        //    TwilioClient.Init(AccountSid, AuthToken);

        //    var message = MessageResource.Create(
        //        body: text,
        //        from: new Twilio.Types.PhoneNumber("whatsapp:" + PhoneNumberFrom),
        //        to: new Twilio.Types.PhoneNumber("whatsapp:" + PhoneNumberTo)
        //    );

        //    Console.WriteLine(message.DateSent);
        //    Console.WriteLine(message.Status);
        //}
    }
}
